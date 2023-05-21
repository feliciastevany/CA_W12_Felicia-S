using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace CA_W12_Felicia_S
{
    public partial class Form1 : Form
    {
        MySqlConnection mysqlConnect;
        MySqlCommand mysqlCommand;
        MySqlDataAdapter mySqlAdapter;
        MySqlDataReader mySqlDataReader;
        string connectionString;
        string sqlQuery;
        public Form1()
        {
            connectionString = "server=localhost;uid=root;pwd=Apakau123;database=premier_league";
            try
            {
                mysqlConnect = new MySqlConnection(connectionString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            InitializeComponent();
        }

        DataTable Datanation = new DataTable();
        DataTable Datateamname = new DataTable();
        DataTable dataManager = new DataTable();
        DataTable dataFreeManager = new DataTable();
        DataTable dataDelete = new DataTable();
        DataTable dataDelete2 = new DataTable();
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;

            dataManager.Clear();
            dataFreeManager.Clear();
            Datanation.Clear();
            Datateamname.Clear();
            dataDelete.Clear();

            dataGridViewManager.DataSource = dataManager;
            dataGridViewFreeManager.DataSource = dataFreeManager;
            dataGridViewDelete.DataSource = dataDelete;

            try
            {
                sqlQuery = "SELECT nationality_id, nation FROM nationality;";
                mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
                mySqlAdapter.Fill(Datanation);
                comboBoxNation.DataSource = Datanation;
                comboBoxNation.ValueMember = "nationality_id";
                comboBoxNation.DisplayMember = "nation";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                sqlQuery = "SELECT team_id, team_name FROM team;";
                mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
                mySqlAdapter.Fill(Datateamname);
                comboBoxTeamname.DataSource = Datateamname;
                comboBoxTeamname.ValueMember = "team_id";
                comboBoxTeamname.DisplayMember = "team_name";

                comboBoxUpdateTeam.DataSource = Datateamname;
                comboBoxUpdateTeam.ValueMember = "team_id";
                comboBoxUpdateTeam.DisplayMember = "team_name";

                comboBoxDeletetim.DataSource = Datateamname;
                comboBoxDeletetim.ValueMember = "team_id";
                comboBoxDeletetim.DisplayMember = "team_name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonAddplay_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string teamnum = textBoxTeamnum.Text;
            string nation = comboBoxNation.Text;
            string pos = textBoxPos.Text;
            string height = textBoxheight.Text;
            string weight = textBoxewight.Text;
            string birthdate = dateTimePicker1.Text;
            string teamname = comboBoxTeamname.Text;
            string playerID = textBoxPlayerID.Text;

            if (textBoxName.Text == "" || textBoxTeamnum.Text == "" || comboBoxNation.SelectedValue.ToString() == "" || textBoxPos.Text == "" || textBoxheight.Text == "" || textBoxewight.Text == "" || dateTimePicker1.Value.ToString() == "" || comboBoxTeamname.SelectedValue.ToString() == "" || textBoxPlayerID.Text == "")
            {
                MessageBox.Show("Please fill the fields correctly");
            }
            else
            {
                sqlQuery = $"INSERT INTO player VALUES ('{textBoxPlayerID.Text}' , {textBoxTeamnum.Text}, '{textBoxName.Text}', '{comboBoxNation.SelectedValue}', '{textBoxPos.Text}', {textBoxheight.Text}, {textBoxewight.Text}, '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}', '{comboBoxTeamname.SelectedValue}', 1, 0);";
                try
                {
                    mysqlConnect.Open();
                    mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                    mysqlCommand.ExecuteNonQuery();
                    mysqlConnect.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    mysqlConnect.Close();
                    textBoxName.Text = "";
                    textBoxTeamnum.Text = "";
                    textBoxPos.Text = "";
                    textBoxewight.Text = "";
                    textBoxheight.Text = "";
                    textBoxPlayerID.Text = "";
                    comboBoxNation.Text = "";
                    comboBoxTeamname.Text = "";
                }
            }
        }

        private void comboBoxUpdateTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataManager = new DataTable();
            dataFreeManager = new DataTable();
            buttonUpdate.Enabled = true;

            sqlQuery = $"SELECT m.manager_name, t.team_name, m.birthdate, n.nation FROM manager m, team t, nationality n WHERE m.manager_id = t.manager_id and n.nationality_id = m.nationality_id and t.team_id = '{comboBoxUpdateTeam.SelectedValue}';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
            mySqlAdapter.Fill(dataManager);
            dataGridViewManager.DataSource = dataManager;

            sqlQuery = "SELECT m.manager_name, n.nation, m.birthdate FROM manager m, nationality n WHERE n.nationality_id = m.nationality_id and working = '0';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
            mySqlAdapter.Fill(dataFreeManager);
            dataGridViewFreeManager.DataSource = dataFreeManager;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            dataManager = new DataTable();
            dataFreeManager = new DataTable();

            string teamid = comboBoxUpdateTeam.SelectedValue.ToString();

            mysqlConnect.Open();
            sqlQuery = $"SELECT t.manager_id FROM team t, manager m, nationality n WHERE m.manager_id = t.manager_id and n.nationality_id = m.nationality_id and t.team_id = '{comboBoxUpdateTeam.SelectedValue}';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            string beforemanager = mysqlCommand.ExecuteScalar().ToString();


            sqlQuery = $"SELECT m.manager_id FROM manager m, nationality n WHERE n.nationality_id = m.nationality_id and working = '0' and m.manager_name = '{dataGridViewFreeManager.CurrentRow.Cells[0].Value}';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            string newmanager = mysqlCommand.ExecuteScalar().ToString();
            mysqlConnect.Close();

            MessageBox.Show(teamid + beforemanager + newmanager);

            // Update team ubah manager lama jadi manager baru
            sqlQuery = $"update team set manager_id = '{newmanager}' WHERE manager_id = '{beforemanager}'";
            mysqlConnect.Open();
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mysqlCommand.ExecuteNonQuery();


            // Manager baru working == 1
            sqlQuery = $"update manager set working = 0 WHERE manager_id = '{beforemanager}'";

            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mysqlCommand.ExecuteNonQuery();


            // Manager lama working == 0
            sqlQuery = $"update manager set working = 1 WHERE manager_id = '{newmanager}'";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mysqlCommand.ExecuteNonQuery();
            mysqlConnect.Close();

            sqlQuery = $"SELECT m.manager_name, t.team_name, m.birthdate, n.nation FROM manager m, team t, nationality n WHERE m.manager_id = t.manager_id and n.nationality_id = m.nationality_id and t.team_id = '{comboBoxDeletetim.SelectedValue}';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
            mySqlAdapter.Fill(dataManager);
            dataGridViewManager.DataSource = dataManager;

            sqlQuery = "SELECT m.manager_name, n.nation, m.birthdate FROM manager m, nationality n WHERE n.nationality_id = m.nationality_id and working = 0;";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
            mySqlAdapter.Fill(dataFreeManager);
            dataGridViewFreeManager.DataSource = dataFreeManager;
        }
        private void comboBoxDeletetim_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataDelete.Clear();
            buttonDelete.Enabled = true;

            try
            {
                sqlQuery = $"SELECT p.team_id, p.player_name as 'Name', n.nation as 'Nationality', p.playing_pos as 'Playing Position', p.team_number as 'Number', height as 'Height', p.weight as 'Weight', p.birthdate as 'Birthdate' FROM player p, nationality n WHERE p.nationality_id = n.nationality_id and p.status = 1 and p.team_id = '{comboBoxDeletetim.SelectedValue}';";
                mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
                mySqlAdapter.Fill(dataDelete);
                dataGridViewDelete.DataSource = dataDelete;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string playerchoose = dataGridViewDelete.CurrentRow.Cells[0].Value.ToString();

            int count = 0;
            sqlQuery = $"SELECT p.player_name as 'Name', n.nation as 'Nationality', p.playing_pos as 'Playing Position', p.team_number as 'Number', p.height as 'Height', p.weight as 'Weight', p.birthdate as 'Birthdate', p.status FROM player p, nationality n WHERE p.nationality_id = n.nationality_id and p.status = 1 and p.team_id = '{comboBoxDeletetim.SelectedValue}';";
            mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
            mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
            mySqlAdapter.Fill(dataDelete2);

            for (int i = 0; i < dataDelete2.Rows.Count; i++)
            {
                if (dataDelete2.Rows[i][8].ToString() == "1")
                {
                    count++;
                }
            }
            if (count <= 11)
            {
                MessageBox.Show("If you want to remove, player must be greater than 11");
            }
            else if (playerchoose == "")
            {
                MessageBox.Show("Please choose a player");
            }
            else
            {
                sqlQuery = $"UPDATE player set `status` = 0 WHERE player_id = '{playerchoose}';";
                try
                {
                    mysqlConnect.Open();
                    mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                    mySqlDataReader = mysqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    mysqlConnect.Close();
                    dataDelete.Clear();
                    try
                    {
                        sqlQuery = $"SELECT p.player_name as 'Name', n.nation as 'Nationality', p.playing_pos as 'Playing Position', p.team_number as 'Number', height as 'Height', p.weight as 'Weight', p.birthdate as 'Birthdate' FROM player p, nationality n WHERE p.nationality_id = n.nationality_id and p.status = 1 and p.team_id = '{comboBoxUpdateTeam.SelectedValue}';";
                        mysqlCommand = new MySqlCommand(sqlQuery, mysqlConnect);
                        mySqlAdapter = new MySqlDataAdapter(mysqlCommand);
                        mySqlAdapter.Fill(dataDelete);
                        dataGridViewDelete.DataSource = dataDelete;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    buttonDelete.Enabled = false;
                }
            }
        }
        private void dataGridViewDelete_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
