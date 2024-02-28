using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDeportes.Models;
using static functional_tests.TestServices;

namespace functional_tests.Pages
{
    public class HomePage
    {
        private IWebDriver Driver;

        /// <summary>
        /// Modelo de prueba.
        /// </summary>
        public DeporteModel modeloTest = new DeporteModel {
            Nombre = "Deporte de prueba",
            PaisOrigen = "Costa Rica",
            CaracteristicaPrincipal = "Se juega con la mano",
            JugadoresPorEquipo = 5,
            EsOlimpico = true
        };

        /// <summary>
        /// Constructor. Inicializa driver.
        /// </summary>
        /// <param name="driver"></param>
        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// Atributo que representa el botón para agregar un nuevo deporte.
        /// </summary>
        public IWebElement BotonAgregarDeporte
        {
            get
            {
                return EsperarElemento(By.CssSelector(".boton-agregar"), Driver);
            }
        }

        /// <summary>
        /// Atributo que representa una entrada de texto para ingresar el nombre del deporte.
        /// </summary>
        public IWebElement EntradaNombreDeporte
        {
            get
            {
                return EsperarElemento(By.Id("Nombre"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa una entrada de texto para ingresar el país de origen del deporte.
        /// </summary>
        public IWebElement EntradaPaisOrigenDeporte
        {
            get
            {
                return EsperarElemento(By.Id("PaisOrigen"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa una entrada de texto para ingresar la característica principal del deporte.
        /// </summary>
        public IWebElement EntradaCaracteristicaPrincipalDeporte
        {
            get
            {
                return EsperarElemento(By.Id("CaracteristicaPrincipal"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa una entrada de texto para ingresar la cantidad de jugadores por equipo del deporte.
        /// </summary>
        public IWebElement EntradaJugadoresPorEquipoDeporte
        {
            get
            {
                return EsperarElemento(By.Id("JugadoresPorEquipo"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa un checkbox para indicar si el deporte es olímpico.
        /// </summary>
        public IWebElement EntradaEsOlimpicoDeporte
        {
            get
            {
                return EsperarElemento(By.Id("EsOlimpico"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa el botón de "Agregar" del formulario.
        /// </summary>
        public IWebElement BotonFormAgregar
        {
            get
            {
                return EsperarElemento(By.CssSelector(".btn"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa el botón de "Volver al inicio".
        /// </summary>
        public IWebElement BotonVolverAlInicio
        {
            get
            {
                return EsperarElemento(By.CssSelector(".btn-primary"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Obtiene el botón de editar el reporte predeterminado.
        /// </summary>
        /// <param name="identificador">Número de hijo en css</param>
        /// <returns></returns>
        public IWebElement getBotonActualizarDeporte(int identificador)
        {
            string byIdentificador = $"tr:nth-child({identificador}) .boton-editar";
            return EsperarElemento(By.CssSelector(byIdentificador), Driver);  // Método de TestService.cs
        }

        /// <summary>
        /// Obtiene el botón de eliminar el reporte predeterminado.
        /// </summary>
        /// <param name="identificador">Número de hijo en css</param>
        /// <returns></returns>
        public IWebElement getBotonEliminarDeporte(int identificador)
        {
            string byIdentificador = $"tr:nth-child({identificador}) .boton-eliminar";
            return EsperarElemento(By.CssSelector(byIdentificador), Driver);  // Método de TestService.cs
        }

        /// <summary>
        /// Atributo que representa el título de la tabla de la lista de los deportes.
        /// </summary>
        public IWebElement TituloTabla
        {
            get
            {
                return EsperarElemento(By.CssSelector(".card-title"), Driver);  // Método de TestService.cs
            }
        }

        /// <summary>
        /// Atributo que representa el título de la página.
        /// </summary>
        public string TituloDePagina
        {
            get
            {
                return Driver.Title;
            }
        }

        /// <summary>
        /// Busca el nombre del deporte de testing y devuelve la posición (child).
        /// Los child comienzan en 1.
        /// </summary>
        /// <returns>Número de hijo en css</returns>
        public int BuscarIdentificadorDeporte(string nombreDeporte)
        {
            bool encontrado = false;
            int cantidadDeportes = ContarDeportes();  // Método de TestService.cs
            int deporte = 1;
            for (; deporte <= cantidadDeportes && encontrado == false; ++deporte)
            {
                string byActual = $"tr:nth-child({deporte}) > .align-middle:nth-child(1)";
                IWebElement nombreActual = EsperarElemento(By.CssSelector(byActual), Driver);  // Método de TestService.cs

                if (nombreActual.Text == nombreDeporte)
                {
                    encontrado = true;
                }
            }
            if (encontrado == false) return 0;
            return --deporte;
        }

        /// <summary>
        /// Indica si el deporte predeterminado fue encontrado o no.
        /// </summary>
        /// <returns>True en caso de que lo haya encontrado, false en caso contrario.</returns>
        public bool DeporteLeido()
        {
            int identificador = BuscarIdentificadorDeporte(modeloTest.Nombre);
            if (identificador != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Identifica el elemento principal de la página (título de tabla) y el título de la página.
        /// </summary>
        /// <param name="tituloTablaEsperado">Cadena que debería decir el elemento</param>
        /// <returns>True en caso que la página haya cargado con los datos correctos. False en caso contrario</returns>
        public bool PaginaLeida(string tituloTablaEsperado)
        {
            bool tituloPaginaEsCorrecto = TituloDePagina != null;
            bool tituloTablaEsCorrecto = TituloTabla.Text == tituloTablaEsperado;
            return tituloPaginaEsCorrecto && tituloTablaEsCorrecto;
        }

        /// <summary>
        /// Agrega un deporte de prueba.
        /// </summary>
        public void AgregarDeportePredeterminado()
        {
            BotonAgregarDeporte.Click();
            EntradaNombreDeporte.SendKeys(modeloTest.Nombre);
            EntradaPaisOrigenDeporte.SendKeys(modeloTest.PaisOrigen);
            EntradaCaracteristicaPrincipalDeporte.SendKeys(modeloTest.CaracteristicaPrincipal);
            EntradaJugadoresPorEquipoDeporte.SendKeys(modeloTest.JugadoresPorEquipo.ToString());
            if (modeloTest.EsOlimpico)
            {
                EntradaEsOlimpicoDeporte.Click();
            }
            BotonFormAgregar.Click();
            BotonVolverAlInicio.Click();
        }

        /// <summary>
        /// Elimina el deporte de prueba.
        /// </summary>
        public void EliminarDeporte(string nombreDeporte)
        {
            int identificador = BuscarIdentificadorDeporte(nombreDeporte);
            if (identificador != 0)
            {
                getBotonEliminarDeporte(identificador).Click();
                Driver.SwitchTo().Alert().Accept();
                BotonVolverAlInicio.Click();
            }
            else
            {
                Console.WriteLine("No se pudo eliminar. Deporte predeterminado no encontrado");
            }
        }

        /// <summary>
        /// Actualiza el nombre del deporte de prueba.
        /// </summary>
        /// <param name="nombreNuevo">Nombre que se le quiere asignar.</param>
        public void ActualizarNombreDeporte(string nombreNuevo)
        {
            int identificador = BuscarIdentificadorDeporte(modeloTest.Nombre);
            if (identificador != 0)
            {
                getBotonActualizarDeporte(identificador).Click();
                Driver.SwitchTo().Alert().Accept();
                EntradaNombreDeporte.Clear();
                EntradaNombreDeporte.SendKeys(nombreNuevo);
                BotonFormAgregar.Click();
                BotonVolverAlInicio.Click();
            }
            else
            {
                Console.WriteLine("No se pudo actualizar. Deporte predeterminado no encontrado");
            }
        }

    }
}
