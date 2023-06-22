using WebDeportes.Models;
using WebDeportes.Controllers;
using WebDeportes.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace unit_test
{
    [TestClass]
    public class DeporteUnitTest
    {
        private DeporteModel? modeloTest;
        private DeporteController? deporteController;
        private DeporteHandler? deporteHandler;

        [TestInitialize]
        public void Initialize()
        {
            deporteController = new();
            deporteHandler = new("BaseDeportes");
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "AgregarDeporte" del controlador funcione correctamente con un modelo válido.

        /// Justificación 1) Valores de entrada: modelo con datos válidos, que no generen excepciones.
        /// Justificación 2) Los resultados esperados son que retorne la vista "AgregarDeporte" con el un mensaje en el viewbag que indique que el deporte fue agregado. Esto verifica que se agregó correctamente.
        /// </summary>
        [TestMethod]
        public void AgregarDeporte_ModeloValido_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "El deporte ha sido agregado.";
            string viewResultEsperado = "AgregarDeporte";

            // crea modelo de prueba
            modeloTest = new DeporteModel
            {
                Nombre = "Deporte de prueba. unit Test",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true
            };

            // action
            var viewResult = this.deporteController.AgregarDeporte(modeloTest) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "AgregarDeporte" del controlador funcione correctamente y dé un error al agregar un modelo con un nombre ya existente.

        /// Justificación 1) Valores de entrada: modelo con nombre ya existente, para que genere ViewBag con mensaje de error al usuario.
        /// Justificación 2) Los resultados esperados son que retorne la vista "AgregarDeporte" con el un mensaje en el viewbag que indique que el deporte NO fue agregado debido a que su nombre está repetido.
        /// </summary>
        [TestMethod]
        public void AgregarDeporte_NombreRepetido_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "No se pudo agregar el deporte.\nYa existe un deporte con ese nombre.";
            string viewResultEsperado = "AgregarDeporte";

            // Crea modelo de prueba 1
            modeloTest = new DeporteModel
            {
                Nombre = "Deporte de prueba. unit Test",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true
            };

            // Agrega el primer modelo válido.
            this.deporteController.AgregarDeporte(modeloTest);

            // Crea modelo de prueba 2. Nombre repetido
            modeloTest = new DeporteModel
            {
                Nombre = "Deporte de prueba. unit Test",
                PaisOrigen = "Puerto Rico",
                CaracteristicaPrincipal = "Se juega con el pie",
                JugadoresPorEquipo = 6,
                EsOlimpico = false
            };

            // action
            var viewResult = this.deporteController.AgregarDeporte(modeloTest) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "EliminarDeporte" del controlador funcione correctamente con un modelo existente.

        /// Justificación 1) Valores de entrada: modelo ya existente en la BD, para que se pueda eliminar y no genere excepciones.
        /// Justificación 2) Los resultados esperados son que retorne la vista "EliminarDeporte" con el un mensaje en el viewbag que indique que el deporte fue eliminado. Esto verifica que se eliminó correctamente.
        /// </summary>
        [TestMethod]
        public void EliminarDeporte_ModeloExistente_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "El deporte ha sido eliminado.";
            string viewResultEsperado = "EliminarDeporte";

            // Crea modelo de prueba
            modeloTest = new DeporteModel
            {
                Nombre = "Deporte de prueba. unit Test",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true
            };

            // Agrega el modelo.
            this.deporteController.AgregarDeporte(modeloTest);

            // Obtiene el identificador que le dio la BD.
            int ID_Deporte = deporteHandler.ObtenerDeportes().Find(x => x.Nombre == modeloTest.Nombre).ID;

            // action
            var viewResult = this.deporteController.EliminarDeporte(ID_Deporte) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "EliminarDeporte" del controlador funcione correctamente y dé un error al eliminar un modelo con un ID NO existente.

        /// Justificación 1) Valores de entrada: modelo que no existe en la BD, para que genere ViewBag con mensaje de error al usuario.
        /// Justificación 2) Los resultados esperados son que retorne la vista "EliminarDeporte" con el un mensaje en el viewbag que indique que el deporte NO fue eliminado debido a que su ID no existe.
        /// </summary>
        [TestMethod]
        public void EliminarDeporte_ModeloInexistente_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "No existe un deporte con ese ID.";
            string viewResultEsperado = "EliminarDeporte";
            int ID_Inexistente = -1;

            // action
            var viewResult = this.deporteController.EliminarDeporte(ID_Inexistente) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "ActualizarDeporte" del controlador funcione correctamente con un modelo existente.

        /// Justificación 1) Valores de entrada: modelo ya existente en la BD pero con un nombre distinto, para que se pueda actualizar y no genere excepciones.
        /// Justificación 2) Los resultados esperados son que retorne la vista "ActualizarDeporte" con el un mensaje en el viewbag que indique que el deporte fue actualizado. Esto verifica que se actualizó correctamente.
        /// </summary>
        [TestMethod]
        public void ActualizarDeporte_ModeloValido_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "El deporte ha sido actualizado.";
            string viewResultEsperado = "ActualizarDeporte";

            // Crea modelo de prueba
            modeloTest = new DeporteModel
            {
                Nombre = "Nombre que se actualizará",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true
            };

            // Agrega el modelo.
            this.deporteController.AgregarDeporte(modeloTest);

            // Obtiene el identificador que le dio la BD.
            int ID_Deporte = deporteHandler.ObtenerDeportes().Find(x => x.Nombre == modeloTest.Nombre).ID;

            // Actualiza el nombre y el ID del modelo local
            modeloTest.Nombre = "Deporte de prueba. unit Test";
            modeloTest.ID = ID_Deporte;

            // action
            var viewResult = this.deporteController.ActualizarDeporte(modeloTest) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "ActualizarDeporte" del controlador funcione correctamente y dé un error al actualizar un modelo con un ID NO existente.

        /// Justificación 1) Valores de entrada: modelo que no existe en la BD, para que genere ViewBag con mensaje de error al usuario.
        /// Justificación 2) Los resultados esperados son que retorne la vista "ActualizarDeporte" con el un mensaje en el viewbag que indique que el deporte NO fue actualizado debido a que su ID no existe.
        /// </summary>
        [TestMethod]
        public void ActualizarDeporte_ModeloInexistente_RetornaViewConViewBagEspecifico()
        {
            // arrange
            // Resultados esperados
            string viewBagEsperado = "No se pudo actualizar el deporte.";
            string viewResultEsperado = "ActualizarDeporte";

            // Crea modelo de prueba
            modeloTest = new DeporteModel
            {
                Nombre = "Nombre que se actualizará",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true,
                ID = -1
            };

            // Agrega el modelo.
            this.deporteController.AgregarDeporte(modeloTest);

            // action
            var viewResult = this.deporteController.ActualizarDeporte(modeloTest) as ViewResult;

            // assert
            Assert.AreEqual(viewResultEsperado, viewResult.ViewName);
            Assert.AreEqual(viewBagEsperado, deporteController.ViewBag.Message);
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "AgregarDeporte" del handler funcione correctamente y dé una excepción al agregar un modelo con un nombre ya existente.

        /// Justificación 1) Valores de entrada: modelo con nombre ya existente, para que genere Excepción con mensaje de error.
        /// Justificación 2) El resultado esperado es que lance una excepción que indique que no se pudo agregar el deporte ya que su nombre está repetido.
        /// </summary>
        [TestMethod]
        public void AgregarDeporteHandler_NombreRepetido_LanzaExcepcion()
        {
            // arrange
           string msgExcepcionEsperado = "No se pudo agregar el deporte. Nombre repetido.";
            try
            {
                // arrange
                // Crea modelo de prueba 1
                modeloTest = new DeporteModel
                {
                    Nombre = "Deporte de prueba. unit Test",
                    PaisOrigen = "Costa Rica",
                    CaracteristicaPrincipal = "Se juega con la mano",
                    JugadoresPorEquipo = 5,
                    EsOlimpico = true
                };

                // Agrega el primer modelo válido.
                this.deporteHandler.AgregarDeporte(modeloTest);

                // Crea modelo de prueba 2. Nombre repetido
                modeloTest = new DeporteModel
                {
                    Nombre = "Deporte de prueba. unit Test",
                    PaisOrigen = "Puerto Rico",
                    CaracteristicaPrincipal = "Se juega con el pie",
                    JugadoresPorEquipo = 6,
                    EsOlimpico = false
                };

                // action
                this.deporteHandler.AgregarDeporte(modeloTest);
                Assert.Fail("No se obtuvo ninguna excepción cuando se esperaba al menos una");
            } catch (Exception e)
            {
                Assert.AreEqual(msgExcepcionEsperado, e.Message);
            }
        }

        /// <summary>
        /// El objetivo de la prueba es verificar que la función "AgregarDeporte" del handler funcione correctamente con un modelo válido.

        /// Justificación 1) Valores de entrada: modelo con datos válidos, que no generen excepciones ni errores.
        /// Justificación 2) Los resultados esperados son que retorne un entero "1" que indique que una fila de la tabla fue afectada. Esto verifica que se agregó correctamente.
        /// </summary>
        [TestMethod]
        public void AgregarDeporteHandler_ModeloValido_RetornaUnaFilaAfectada()
        {
            // arrange
            // Resultados esperados
            int filasAfectadasEsperadas = 1;
            int filasAfectadasResultado;

            // crea modelo de prueba
            modeloTest = new DeporteModel
            {
                Nombre = "Deporte de prueba. unit Test",
                PaisOrigen = "Costa Rica",
                CaracteristicaPrincipal = "Se juega con la mano",
                JugadoresPorEquipo = 5,
                EsOlimpico = true
            };

            // action
            filasAfectadasResultado = this.deporteHandler.AgregarDeporte(modeloTest);

            // assert
            Assert.AreEqual(filasAfectadasEsperadas, filasAfectadasResultado);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Elimina el modelo de testing en caso que fuese creado.
            if (modeloTest != null)
            {
                var deporteAgregado = deporteHandler.ObtenerDeportes().Find(x => x.Nombre == modeloTest.Nombre);
                if (deporteAgregado != null)
                {
                    deporteHandler.EliminarDeporte(deporteAgregado.ID);
                }
            }
        }
    }
}