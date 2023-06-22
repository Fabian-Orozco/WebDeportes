using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebDeportes.Handlers;
using WebDeportes.Models;

namespace WebDeportes.Controllers
{
    /// <summary>
    /// Controlador de la clase de deportes.
    /// </summary>
    public class DeporteController : Controller
    {
        private DeporteHandler _DeporteHandler = new DeporteHandler("BaseDeportes");

        /// <summary>
        /// Recupera los deportes de la base de datos y despliega la vista principal con la tabla de los deportes.
        /// </summary>
        /// <returns>Vista principal con la tabla de deportes</returns>
        public IActionResult Index()
        {
            List<DeporteModel> deportes = _DeporteHandler.ObtenerDeportes();
            return View(deportes);
        }

        /// <summary>
        /// Retorna una vista con el formulario para agregar el deporte.
        /// </summary>
        [HttpGet]
        public IActionResult AgregarDeporte()
        {
            return View();
        }

        /// <summary>
        /// Permite agregar un deporte a la base de datos.
        /// </summary>
        /// <param name="deporte">Modelo del deporte a agregar en la base de datos</param>
        /// <returns>Redirecciona a la vista del formulario con un mensaje de exito </returns>
        [HttpPost]
        public IActionResult AgregarDeporte(DeporteModel deporte)
        {
            // Se da por hecho el caso de error.
            ViewBag.Message = null;
            ViewBag.ExitoAlCrear = false;

            try
            {
                if (ModelState.IsValid)
                {
                    int filasAfectadas = _DeporteHandler.AgregarDeporte(deporte);
                    // Caso exitoso
                    if (filasAfectadas > 0)
                    {
                        ViewBag.Message = "El deporte ha sido agregado.";
                        ViewBag.ExitoAlCrear = true;
                    }
                }
            }
            catch (DuplicateNameException e)
            {
                ViewBag.Message = "No se pudo agregar el deporte.\nYa existe un deporte con ese nombre.";
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo agregar el deporte. Exception caught. ", e.Message);
            }
            return View("AgregarDeporte");
        }

        /// <summary>
        /// Dirige a la vista que indica el resultado del borrado.
        /// </summary>
        /// <param name="ID">ID del deporte a eliminar</param>
        /// <returns>Vista que indica el resultado del borrado.</returns>
        [HttpGet]
        public IActionResult EliminarDeporte(int ID)
        {
            // Se da por hecho el caso de error.
            ViewBag.Message = null;
            ViewBag.ExitoAlEliminar = false;
            try
            {
                DeporteModel deporte = _DeporteHandler.ObtenerDeportes().Find(model => model.ID == ID);

                if (deporte != null)
                {
                    int filasAfectadas = _DeporteHandler.EliminarDeporte(deporte.ID);
                    // Caso exitoso.
                    if (filasAfectadas > 0)
                    {
                        ViewBag.Message = "El deporte ha sido eliminado.";
                        ViewBag.ExitoAlEliminar = true;

                    }
                    else
                    {
                        ViewBag.Message = "No se pudo eliminar el deporte.";
                    }
                }
                else
                {
                    ViewBag.Message = "No existe un deporte con ese ID.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo eliminar el deporte. Exception caught. ", e.Message);
            }
            return View("EliminarDeporte");
        }

        /// <summary>
        /// Retorna una vista con el formulario para actualizar el deporte.
        /// </summary>
        /// <param name="ID"> Identificador del modelo </param>
        /// <returns>Vista del formulario con el deporte a actualizar</returns>
        [HttpGet]
        public IActionResult ActualizarDeporte(int ID)
        {
            DeporteModel deporteAnterior = new();
            try
            {
                deporteAnterior = _DeporteHandler.ObtenerDeportes().Find(model => model.ID == ID);
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo actualizar el deporte. Exception caught. ", e.Message);
            }
            if (deporteAnterior == null)
            {
                return RedirectToAction("Index", "Deporte");
            }
            return View(deporteAnterior);
        }

        /// <summary>
        /// Actualiza los datos de un deporte.
        /// </summary>
        /// <param name="deporteActualizado"> Modelo con los datos actualizados </param>
        /// <returns> Vista con el formulario y el estado del resultado de la acción anterior </returns>
        [HttpPost]
        public IActionResult ActualizarDeporte(DeporteModel deporteActualizado)
        {
            // Se da por hecho el caso de error.
            ViewBag.Message = null;
            ViewBag.ExitoAlActualizar = false;
            try
            {
                int filasAfectadas = _DeporteHandler.ActualizarDeporte(deporteActualizado);
                // Caso exitoso
                if (filasAfectadas > 0)
                {
                    ViewBag.Message = "El deporte ha sido actualizado.";
                    ViewBag.ExitoAlActualizar = true;

                }
                else
                {
                    ViewBag.Message = "No se pudo actualizar el deporte.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo actualizar el deporte. Exception caught. ", e.Message);
            }
            return View("ActualizarDeporte");
        }
    }
}