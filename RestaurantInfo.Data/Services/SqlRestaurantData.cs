using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantInfo.Data.Services
{
    public class SqlRestaurantData : IRestaurantData
    {
        List<Restaurant> restaurants;
        public SqlRestaurantData()
        {
            restaurants = new List<Restaurant>();
        }
        public void Add(Restaurant restaurant)
        {
            try
            {
                using (var connection = new SqlConnection(Helper.GetConnectionString("OdeToFoodDb")))
                {
                    using (var command = new SqlCommand("AddRestaurant", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@RestName", restaurant.Name));
                        command.Parameters.Add(new SqlParameter("@RestLocation", restaurant.Location));
                        command.Parameters.Add(new SqlParameter("@Cuisine", restaurant.Cuisine));

                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@RestId",
                            Value = restaurant.Id,
                            IsNullable = false,
                            DbType = DbType.Int32,
                            Direction = ParameterDirection.Output
                        });

                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.ExecuteNonQuery();

                        restaurant.Id = (int)command.Parameters["@RestId"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(Helper.GetConnectionString("OdeToFoodDb")))
                {
                    using (var command = new SqlCommand("DeleteRestaurant", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@RestId", id));
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Restaurant Get(int id)
        {
            var restaurant = new Restaurant();
            try
            {
                using (var connection = new SqlConnection(Helper.GetConnectionString("OdeToFoodDb")))
                {
                    using (var command = new SqlCommand("GetRestaurantById", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@RestId", id));
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                restaurant.Id = Convert.ToInt32(dr["RestId"].ToString());
                                restaurant.Name = dr["RestName"].ToString();
                                restaurant.Location = dr["RestLocation"].ToString();
                                restaurant.Cuisine = dr["Cuisine"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return restaurant;
        }

        public IEnumerable<Restaurant> GetAll()
        {

            try
            {
                using (var connection = new SqlConnection(Helper.GetConnectionString("OdeToFoodDb")))
                {
                    using (var command = new SqlCommand("GetRestaurants", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                var restaurant = new Restaurant
                                {
                                    Id = Convert.ToInt32(dr["RestId"].ToString()),
                                    Name = dr["RestName"].ToString()
                                };
                                restaurants.Add(restaurant);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return restaurants.OrderBy(r => r.Name);
        }

        public void Update(Restaurant restaurant)
        {
            try
            {
                using (var connection = new SqlConnection(Helper.GetConnectionString("OdeToFoodDb")))
                {
                    using (var command = new SqlCommand("UpdateRestaurant", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@RestId", restaurant.Id));
                        command.Parameters.Add(new SqlParameter("@RestName", restaurant.Name));
                        command.Parameters.Add(new SqlParameter("@RestLocation", restaurant.Location));
                        command.Parameters.Add(new SqlParameter("@Cuisine", restaurant.Cuisine));

                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }

}

