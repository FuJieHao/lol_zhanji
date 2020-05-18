using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace lol
{
    public partial class Default : System.Web.UI.Page
    {

        protected void create_data(object sender, EventArgs e)
        {
            this.delete.Visible = true;
            ViewState["row"] = Convert.ToInt16(ViewState["row"]) + 1;

            player[] players = load_data();
            create_row(players[Convert.ToInt16(ViewState["row"])-1]);
            if (Convert.ToInt16(ViewState["row"]) >= players.Length)
            {
                Response.Write("警告⚠️:数据库没有足够的数据显示");
                this.create.Visible = false;
            }
        }

        protected void delete_data(object sender, EventArgs e)
        {
            this.create.Visible = true;
            this.table_data.Rows.RemoveAt(this.table_data.Rows.Count - 1);
            ViewState["row"] = Convert.ToInt16(ViewState["row"]) - 1;
            if(Convert.ToInt16(ViewState["row"]) ==0)
            {
                this.delete.Visible = false;
            }
        }

        void create_row(player player)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            Image hero_img = new Image();
            hero_img.Attributes.CssStyle.Value = "border-radius:50%;width:65px;height:65px;margin-left:10px;";
            hero_img.ImageUrl = "hero/" + player.hero_img;

            Label zhs_name = new Label();
            zhs_name.Attributes.CssStyle.Value = "font-size:15px;position:relative;top:-50px;left:9px";
            zhs_name.Text = player.name;

            Label gold = new Label();
            gold.Attributes.CssStyle.Value = "font-size:12px;position:relative;top:-50px;left:18px";
            gold.Text = "金币：";

            Label gold_num = new Label();
            gold_num.Attributes.CssStyle.Value = "font-size:12px;position:relative;top:-50px;left:18px";
            gold_num.Text = player.gold;

            Label KDA = new Label();
            KDA.Attributes.CssStyle.Value = "font-size:12px;position:relative;top:-50px;left:27px";
            KDA.Text = "KDA：";

            Label KDA_num = new Label();
            KDA_num.Attributes.CssStyle.Value = "font-size:12px;position:relative;top:-50px;left:27px";
            KDA_num.Text = player.kda;

            Label game_type = new Label();
            game_type.Attributes.CssStyle.Value = "font-size:15px;position:relative;top:-8px;left:36px";
            game_type.Text = "单双排";

            Table equip_imgs = equip_list(player.eq);
            equip_imgs.Width = 180;
            equip_imgs.Height = 30;

            Panel panel = new Panel();
            panel.Controls.Add(hero_img);
            panel.Controls.Add(zhs_name);
            panel.Controls.Add(gold);
            panel.Controls.Add(gold_num);
            panel.Controls.Add(KDA);
            panel.Controls.Add(KDA_num);
            panel.Controls.Add(game_type);
            panel.Controls.Add(equip_imgs);

            cell.Controls.Add(panel);
            row.Controls.Add(cell);
            this.table_data.Rows.AddAt(0, row);
        }

        Table equip_list(string[] str_arr)
        {
            Table equip_table = new Table();
            equip_table.Attributes.CssStyle.Value = "border-collapse:separate;border-spacing:0;position:relative;top:-40px;left:94px";

            int row_count = 6;
            TableRow row = new TableRow();
            for (int i = 0; i < row_count; i++)
            {
                TableCell cell = new TableCell();
                Image equipment = new Image();
                equipment.Attributes.CssStyle.Value = "width:30px;height:30px;";
                equipment.ImageUrl = "eq/" + str_arr[i];
                cell.Controls.Add(equipment);
                row.Cells.Add(cell);
            }
            equip_table.Rows.Add(row);

            return equip_table;
        }

        player[] load_data()
        {
            player[] players = null;

            string sqlconn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

            MySqlConnection myConnect = new MySqlConnection(sqlconn);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = myConnect;

            try
            {
                if(myConnect.State == System.Data.ConnectionState.Closed)
                {
                    myConnect.Open();
                }

                cmd.CommandText = "select count(*) from player";

                int col = Convert.ToInt16(cmd.ExecuteScalar());

                players = new player[col];

                for (int i = 0; i < col; i++)
                {
                    players[i] = new player();
                }


                for (int i = 0; i < col; i++)
                {
                    cmd.CommandText = "select hero_img from player where id=" + (i+1).ToString();
                    players[i].hero_img = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select name from player where id=" + (i + 1).ToString();
                    players[i].name = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select gold from player where id=" + (i + 1).ToString();
                    players[i].gold = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select kda from player where id=" + (i + 1).ToString();
                    players[i].kda = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select type from player where id=" + (i + 1).ToString();
                    players[i].type = cmd.ExecuteScalar().ToString();

                    for (int j = 0; j < players[0].eq.Length; j++)
                    {
                        cmd.CommandText = "select eq" + (j+1).ToString() + " from player where id=" + (i+1).ToString();
                        players[i].eq[j] = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("玩家信息读取错误，错误原因:" + ex.Message);
            }
            finally
            {
                
            }

            return players;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ViewState["row"] != null)
            {
                player[] players = load_data();
 
                    for (int i = 0; i < Convert.ToInt16(ViewState["row"]); i++)
                    {
                        create_row(players[i]);
                    }
                

            }
            else { this.delete.Visible = false; }
        }

        class player
        {
            public string hero_img;
            public string name;
            public string gold;
            public string kda;
            public string type;
            public string[] eq = new string[6];
        }
    }
}


