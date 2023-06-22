using OpenQA.Selenium;
using WebDeportes.Models;
using functional_tests.Pages;
using OpenQA.Selenium.Chrome;
using static functional_tests.TestServices;

namespace functional_test
{
    [TestClass]
    public class DeporteFunctionalTest
    {
        private IWebDriver? Driver;
        public const string URL = "https://localhost:7220";
        private HomePage? _HomePage;

        [TestInitialize]
        public void Initialize()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();

            Driver.Navigate().GoToUrl(URL);
            this._HomePage = new(Driver);
        }

        /// <summary>
        /// Prueba Read.
        /// Indica que encuentra el t�tulo de la tabla, el t�tulo de la p�gina y un deporte agregado.

        /// Justificaci�n 1) El objetivo es buscar que la p�gina cargue (en consecuencia su t�tulo) y muestre el t�tulo de la tabla y el nombre del deporte agregado.
        /// Justificaci�n 2) El resultado esperado es que la p�gina muestre parte de su contenido de forma correcta por lo que comprueba que el t�tulo de la tabla y el nombre del deporte se pueden leer.
        /// </summary>
        [TestMethod]
        public void LeerListaDeDeportes()
        {
            // arrange
            string tituloTablaEsperado = "Lista de deportes";
            _HomePage.AgregarDeportePredeterminado();

            // action
            bool paginaLeida = _HomePage.PaginaLeida(tituloTablaEsperado);
            bool deporteLeido = _HomePage.DeporteLeido(); // modelo predeterminado

            // assert
            Assert.IsTrue(paginaLeida, "La p�gina y su contenido no han sido le�dos");
            Assert.IsTrue(deporteLeido, "No se encontr� el deporte");

            // Custom Cleanup
            _HomePage.EliminarDeporte(_HomePage.modeloTest.Nombre);
        }

        /// <summary>
        /// Comprueba Create.
        /// Justificaci�n 1) El objetivo es a�adir un nuevo deporte y ver si se agrega realmente a la base de datos contando la cantidad de elementos que hay en ella antes y despu�s de la inserci�n.
        /// Justificaci�n 2) El resultado esperado es que la cantidad postinserci�n sea n+1 donde n es la cantidad de elementos que hab�an antes de la inserci�n. Esto verifica que se agreg� el nuevo registro.
        /// </summary>
        [TestMethod]
        public void AgregarDeporte()
        {
            // arrange
            int cantidadDeportesPreInsercion = ContarDeportes();
            int cantidadDeportesPostInsercion;

            // action
            _HomePage.AgregarDeportePredeterminado();
            cantidadDeportesPostInsercion = ContarDeportes();

            // assert
            Assert.IsTrue(cantidadDeportesPostInsercion == cantidadDeportesPreInsercion + 1, "La cantidad de deportes no aument� en 1");
            
            // Custom Cleanup
            _HomePage.EliminarDeporte(_HomePage.modeloTest.Nombre);
        }

        /// <summary>
        /// Comprueba Delete.
        /// Justificaci�n 1) El objetivo es borrar deporte existente y ver si se elimina realmente a la base de datos contando la cantidad de elementos que hay en ella antes y despu�s del eliminado.
        /// Justificaci�n 2) El resultado esperado es que la cantidad postinserci�n sea n-1 donde n es la cantidad de elementos que hab�an antes del eliminado. Esto verifica que se elimin� el registro existente.
        /// </summary>
        [TestMethod]
        public void EliminarDeporte()
        {
            // arrange
            _HomePage.AgregarDeportePredeterminado(); 
            int cantidadDeportesPreEliminado = ContarDeportes();
            int cantidadDeportesPostEliminado;

            // action
            _HomePage.EliminarDeporte(_HomePage.modeloTest.Nombre);
            cantidadDeportesPostEliminado = ContarDeportes();

            // assert
            Assert.IsTrue(cantidadDeportesPostEliminado == cantidadDeportesPreEliminado - 1, "La cantidad de deportes no disminuy� en 1");
        }

        /// <summary>
        /// Comprueba Update.
        /// Justificaci�n 1) El objetivo es editar el nombre de un deporte existente y ver si se actualiza realmente a la base de datos verificando el cambio de acuerdo al ID que es �nico.
        /// Justificaci�n 2) El resultado esperado es que el nombre del deporte cambie. Esto verifica que se actualiz� el registro existente.
        /// </summary>
        [TestMethod]
        public void ActualizarNombreDeporte()
        {
            // arrange
            _HomePage.AgregarDeportePredeterminado();
            string nombreNuevo = "Nuevo nombre de pruebas";

            // Obtiene el identificador de la base de datos que es �nico para cada deporte.
            int identificadorDeporte = ObtenerDeportes().Find(x => x.Nombre == _HomePage.modeloTest.Nombre).ID;

            // action
            _HomePage.ActualizarNombreDeporte(nombreNuevo);
            DeporteModel? deporte = ObtenerDeportes().Find(x => x.ID == identificadorDeporte);

            // assert
            Assert.AreEqual(deporte.Nombre, nombreNuevo ,"El deporte no posee el nombre nuevo");
            
            // Custom Cleanup
            _HomePage.EliminarDeporte(nombreNuevo);
        }

        /// <summary>
        /// Cierra el webdriver.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            Driver.Quit();
        }
    }
}