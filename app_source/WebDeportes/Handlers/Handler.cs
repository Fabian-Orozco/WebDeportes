using System.Data;
using System.Data.SqlClient;

namespace WebDeportes.Handlers
{
    /// <summary>
    /// Handler genérico. Establece la conexión y envía consultas read y write.
    /// Pensado para la extensión (OCP).
    /// </summary>
    public class Handler
    {
        protected SqlConnection Conexion;
        protected string RutaConexion; // connection string, indica a cual base conectarse

        /// <summary>
        /// Establece la conexión con la base de datos indicada en appsettings.json
        /// </summary>
        /// <param name="rutaConexion">Nombre de la base a la cual conectarse</param>
        public Handler(string rutaConexion)
        {
            try
            {
                // Objeto que permite acceder al conection string del appsettings.json           
                var builder = WebApplication.CreateBuilder();
                this.RutaConexion = builder.Configuration.GetConnectionString(rutaConexion);
                this.Conexion = new SqlConnection(this.RutaConexion);
            }
            catch (Exception e)
            {
                Console.WriteLine("La conexión no ha sido establecida. Exception caught. ", e.Message);
            }
        }

        /// <summary>
        /// Realiza una consulta read y retorna la tabla con el resultado
        /// </summary>
        /// <param name="consulta">Consulta a enviar</param>
        /// <returns>La tabla vacía en caso de error. 
        /// La tabla con los resultados obtenidos en caso exitoso</returns>
        protected DataTable EnviarConsultaRead(SqlCommand comandoConsulta)
        {
            try
            {
                // Se crea una tabla para el resultado
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoConsulta);
                DataTable consultaFormatoTabla = new DataTable();

                // Abre conexión y envia la consulta
                Conexion.Open();
                adaptadorParaTabla.Fill(consultaFormatoTabla);

                // Cierra conexión
                Conexion.Close();
                return consultaFormatoTabla;
            }
            catch (Exception e)
            {
                Console.WriteLine("Consulta no enviada. Exception caught. ", e.Message);
                return new DataTable();
            }
        }

        /// <summary>
        /// Realiza una consulta write y retorna la cantidad de filas afectadas
        /// </summary>
        /// <param name="consulta">Consulta a enviar</param>
        /// <returns>Cantidad de filas afectadas</returns>
        protected int EnviarConsultaWrite(SqlCommand comandoConsulta)
        {
            // Cantidad de filas de la tabla afectadas después que se realizó la consulta a la base de datos.
            int filasAfectadas = 0;
            try
            {
                // Abre conexión y envia la consulta
                Conexion.Open();
                filasAfectadas = comandoConsulta.ExecuteNonQuery();

                // Cierra conexión
                Conexion.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Consulta no enviada. Exception caught. ", e.Message);
            }
            return filasAfectadas;
        }
    }
}