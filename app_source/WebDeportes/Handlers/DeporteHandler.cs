using System.Data;
using System.Data.SqlClient;
using WebDeportes.Models;

namespace WebDeportes.Handlers
{
    public class DeporteHandler : Handler
    {
        private const int MAX_LEN_NOMBREDEPORTE = 75;
        private const int MAX_LEN_PAIS = 56;

        /// <summary>
        /// Constructor. Invoca al constructor del padre.
        /// </summary>
        /// <param name="rutaConexion">Nombre de la base de datos</param>
        public DeporteHandler(string rutaConexion) : base(rutaConexion) { }

        /// <summary>
        /// Agrega un deporte a la base de datos.
        /// </summary>
        /// <param name="deporte">Modelo del deporte a agregar</param>
        /// <returns>Cantidad de filas afectadas. 0 significa que no agregó el deporte en la BD</returns>
        public int AgregarDeporte(DeporteModel deporte)
        {
            // Cantidad de filas de la tabla afectadas después que se realizó la consulta a la base de datos.
            int filasAfectadas = 0;

            // Verifica si el nombre ya existe.
            if (ObtenerDeportes().Find(x => x.Nombre == deporte.Nombre) == null)
            {
                try
                {
                    string consulta = "exec AgregarDeporte @Nombre, @PaisOrigen, @CaracteristicaPrincipal, @JugadoresPorEquipo, @EsOlimpico";

                    SqlCommand comando = new(consulta, base.Conexion);

                    // Parametrización segura de valores. Evita SQL Injection.
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, MAX_LEN_NOMBREDEPORTE).Value = deporte.Nombre;
                    comando.Parameters.Add("@PaisOrigen", SqlDbType.VarChar, MAX_LEN_PAIS).Value = deporte.PaisOrigen;
                    comando.Parameters.Add("@CaracteristicaPrincipal", SqlDbType.VarChar, int.MaxValue).Value = deporte.CaracteristicaPrincipal;
                    comando.Parameters.Add("@JugadoresPorEquipo", SqlDbType.Int, int.MaxValue).Value = deporte.JugadoresPorEquipo;
                    comando.Parameters.Add("@EsOlimpico", SqlDbType.Bit, 1).Value = Convert.ToByte(deporte.EsOlimpico);

                    filasAfectadas = base.EnviarConsultaWrite(comando);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Deporte no agregado. Exception caught.", e.Message);
                }
            }
            else
            {
                throw new DuplicateNameException("No se pudo agregar el deporte. Nombre repetido.");
            }
            return filasAfectadas;
        }

        /// <summary>
        /// Elimina el deporte que coincida con el ID de la base de datos.
        /// </summary>
        /// <param name="ID">ID del deporte a eliminar</param>
        /// <returns>Cantidad de filas afectadas. 0 significa que no encontró el deporte en la BD</returns>
        public int EliminarDeporte(int ID)
        {
            // Cantidad de filas de la tabla afectadas después que se realizó la consulta a la base de datos.
            int filasAfectadas = 0;
            try
            {
                string consulta = "exec EliminarDeporte @ID";
                SqlCommand comando = new(consulta, base.Conexion);

                // Parametrización segura de valores. Evita SQL Injection.
                comando.Parameters.Add("@ID", SqlDbType.VarChar, int.MaxValue).Value = ID;
                filasAfectadas = base.EnviarConsultaWrite(comando);
            }
            catch (Exception e)
            {
                Console.WriteLine("Deporte no eliminado. Exception caught.", e.Message);
            }
            return filasAfectadas;
        }

        /// <summary>
        /// Actualiza los datos de un deporte existente.
        /// </summary>
        /// <param name="nombreAnterior"> Sirve para identificar el deporte que será actualizado </param>
        /// <param name="deporteActualizado"> Contiene los datos actualizados que serán actualizados </param>
        /// <returns>Cantidad de filas afectadas. 0 significa que no encontró el deporte en la BD</returns>
        public int ActualizarDeporte(DeporteModel deporteActualizado)
        {
            // Cantidad de filas de la tabla afectadas después que se realizó la consulta a la base de datos.
            int filasAfectadas = 0;
            try
            {
                string consulta = "exec ActualizarDeporte @ID, @Nombre, @PaisOrigen, @CaracteristicaPrincipal, @JugadoresPorEquipo, @EsOlimpico";
                SqlCommand comando = new(consulta, base.Conexion);

                // Parametrización segura de valores. Evita SQL Injection.
                comando.Parameters.Add("@ID", SqlDbType.BigInt, int.MaxValue).Value = deporteActualizado.ID;
                comando.Parameters.Add("@Nombre", SqlDbType.VarChar, MAX_LEN_NOMBREDEPORTE).Value = deporteActualizado.Nombre;
                comando.Parameters.Add("@PaisOrigen", SqlDbType.VarChar, MAX_LEN_PAIS).Value = deporteActualizado.PaisOrigen;
                comando.Parameters.Add("@CaracteristicaPrincipal", SqlDbType.VarChar, int.MaxValue).Value = deporteActualizado.CaracteristicaPrincipal;
                comando.Parameters.Add("@JugadoresPorEquipo", SqlDbType.Int, int.MaxValue).Value = deporteActualizado.JugadoresPorEquipo;
                comando.Parameters.Add("@EsOlimpico", SqlDbType.Bit, 1).Value = Convert.ToByte(deporteActualizado.EsOlimpico);

                filasAfectadas = base.EnviarConsultaWrite(comando);
            }
            catch (Exception e)
            {
                Console.WriteLine("Deporte no actualizado. Exception caught.", e.Message);
            }
            return filasAfectadas;
        }

        /// <summary>
        /// Obtiene la lista de todos los deportes que están en la base de datos.
        /// </summary>
        /// <returns>Lista de todos los deportes en la BD.</returns>
        public List<DeporteModel> ObtenerDeportes()
        {
            List<DeporteModel> deportes = new List<DeporteModel>();
            string consulta = "exec ObtenerDeportes";

            try
            {
                SqlCommand comando = new(consulta, base.Conexion);
                DataTable tablaResultado = base.EnviarConsultaRead(comando);

                // Llena la lista con los valores para cada modelo según el registro recuperado.
                foreach (DataRow row in tablaResultado.Rows)
                {
                    deportes.Add(
                        new DeporteModel
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            PaisOrigen = Convert.ToString(row["PaisOrigen"]),
                            CaracteristicaPrincipal = Convert.ToString(row["CaracteristicaPrincipal"]),
                            JugadoresPorEquipo = Convert.ToInt32(row["JugadoresPorEquipo"]),
                            EsOlimpico = Convert.ToBoolean(row["EsOlimpico"]),
                        }
                    );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudieron obtener los deportes. Exception caught.", e.Message);
            }
            return deportes;
        }
    }




}