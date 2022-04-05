using AppBlocks.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlocks.MySqlService
{
    public class MySqlService : IMySqlService
    {
        public string ConnectionString { get; set; }

        public MySqlService()
        {
        }
        public MySqlService(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString ?? Config.Factory.GetConnectionString(typeof(MySqlService).Namespace) ?? Config.Factory.GetConnectionString("MySqlAppBlocks") ?? Config.Factory.GetConnectionString("MySqlDefaultConnection"));
        }

        public Item Create(Item item)
        {
            //_items.InsertOne(item);
            //return item;
            using MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new("insert into Item", conn);
            int results = cmd.ExecuteNonQuery();
            return Get(item.Id);
        }

        public List<Item> Get()
        {
            //return _items.Find(item => true).ToList();
            List<Item> results = new List<Item>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Item", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    results = Item.FromDataReader(reader).ToList();
                    //while (reader.Read())
                    //{
                    //    list.Add(new Item()
                    //    {
                    //        Id = reader["id"].ToString(),
                    //        Created = Convert.ToDateTime(reader["created"]),
                    //        CreatorId = reader["creatorid"].ToString(),
                    //        Data = reader["data"].ToString(),
                    //        Description = reader["description"].ToString(),
                    //        Edited = Convert.ToDateTime(reader["edited"].ToString()),
                    //        EditorId = reader["editorid"].ToString(),
                    //        Icon = reader["icon"].ToString(),
                    //        Image = reader["image"].ToString(),
                    //        Name = reader["name"].ToString(),
                    //        OwnerId = reader["ownerid"].ToString(),
                    //        ParentId = reader["parentid"].ToString(),
                    //        Source = reader["source"].ToString(),
                    //        Status = reader["status"].ToString(),
                    //        TypeId = reader["typeid"].ToString()
                    //    });
                    //}
                }
            }
            return results;
        }

        public Item Get(string id)
        {
            //return _items.Find(item => item.Id == id).FirstOrDefault();

            List<Item> results = new();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new($"select * from Item where id='{id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    Item.FromDataReader(reader);
                }
            }

            return results?.FirstOrDefault();
        }

        public void Remove(string id)
        {
            //_items.DeleteOne(item => item.Id == id);
            using MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new($"delete from Item where id='{id}'", conn);
            cmd.ExecuteNonQuery();
        }

        public void Update(string id, Item item)
        {
            //_items.ReplaceOne(item => item.Id == id, item);
            using MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new($"update Item set name='{item.Name}' where id='{id}'", conn);
            cmd.ExecuteNonQuery();
        }
    }
}
