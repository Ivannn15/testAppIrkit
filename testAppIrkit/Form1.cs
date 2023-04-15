using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace testAppIrkit
{
    public partial class Form1 : Form
    {

        AppDbContext db = new AppDbContext();
        public
        OpenFileDialog fileDialog = new OpenFileDialog();

        public void ViewItemOnForm()
        {
            var Emplr = db.Employees.FirstOrDefault();

            txtBoxAuthoreId.Text = Emplr.AuthorId.ToString();
            txtBoxContext.Text = Emplr.Content.ToString();
            txtBoxFirstName.Text = Emplr.FirstName.ToString();
            txtBoxId.Text = Emplr.Id.ToString();
            txtBoxKindName.Text = Emplr.KindName.ToString();
            txtBoxPerformer.Text = Emplr.Performer.ToString();
            txtBoxReferenceList.Text = Emplr.ReferenceList.ToString();
            txtBoxRegDate.Text = Emplr.RegDate.ToString();
        }

        public Form1()
        {
            InitializeComponent();
            ViewItemOnForm();
        }

        private void btnAddXml_Click(object sender, EventArgs e)
        {
            try
            {
                // Открываем диалоговое окно
                if (fileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                string filename = fileDialog.FileName; // Считываем путь к файлу

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                Employee employee = new Employee();
                XmlNodeList nodes = xmlDocument.SelectNodes("//CardDocument");

                foreach (XmlNode node in nodes)
                {
                    // Поиск максимального id
                    int? MaxID = db.Employees.Max(p => p.Id);
                    if (MaxID == null)
                    {
                        employee.Id = 1;
                    }
                    else
                    {
                        employee.Id = (int)MaxID + 1;
                    }
                    // Присвоение атрибутов xml файла в сущность класса employee
                    employee.FirstName = xmlDocument.SelectSingleNode("//MainInfo/@FirstName").Value;
                    employee.RegDate = DateTime.Parse(node.SelectSingleNode("//MainInfo/@RegDate").Value);
                    employee.Content = node.SelectSingleNode("//MainInfo/@Content").Value;
                    employee.KindName = node.SelectSingleNode("//System/@Kind_Name").Value;
                    employee.ReferenceList = node.SelectSingleNode("//MainInfo/@ReferenceList").Value;
                    employee.AuthorId = node.SelectSingleNode("//MainInfo/@Author").Value;

                    XmlElement? xmlElement = xmlDocument.DocumentElement;
                    XmlNodeList? personNodes = xmlElement?.SelectNodes("//EmployeesRow");

                    foreach (XmlNode _node in personNodes) // Вычисление должности сотрудника по полю RowID
                    {
                        if (employee.AuthorId == _node.SelectSingleNode("@RowID")?.Value)
                        {
                            employee.Performer = _node.SelectSingleNode("@PositionName")?.Value;
                        }
                    }
                    // Добавление и сохранение нового сотрудника в базу данных
                    db.Employees.Add(employee);
                    var temp = db.Employees.Where(p => p.AuthorId == employee.AuthorId); // Проверка на наличие дублирующихся сущностей в бд (если такая сущность уже имеется в базе то выйдет ошибка)
                    if (temp != null)
                    {
                        throw new ArgumentException("Сущность уже существует в базе данных");
                    }
                    else
                    {
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message.ToString(), "Ошибка");
            }

        }
    }
}
