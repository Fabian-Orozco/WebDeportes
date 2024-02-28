using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using WebDeportes.Handlers;
using WebDeportes.Models;

namespace functional_tests
{
    public class TestServices
    {
        /// <summary>
        /// Espera a que el webdriver encuentre un elemento.
        /// </summary>
        /// <param name="identificador">Identificador del elemento</param>
        /// <param name="driver"> Driver que maneja la conexión </param>
        /// <returns>Elemento solicitado</returns>
        static public IWebElement EsperarElemento(By identificador, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.Until(condition =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException ex)
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }
                catch (TimeoutException e)
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }

            });
            Thread.Sleep(500);

            return driver.FindElement(identificador);
        }

        /// <summary>
        /// Cuenta la cantidad de deportes existentes en la BD.
        /// </summary>
        /// <returns>Cantidad de deportes</returns>
        static public int ContarDeportes()
        {
            DeporteHandler deporteHandler = new("BaseDeportes");
            return deporteHandler.ObtenerDeportes().Count();
        }

        /// <summary>
        /// Retorna la lista obtenida por el handler
        /// </summary>
        /// <returns>Cantidad de deportes</returns>
        static public List<DeporteModel> ObtenerDeportes()
        {
            DeporteHandler deporteHandler = new("BaseDeportes");
            return deporteHandler.ObtenerDeportes();
        }
    }
}
