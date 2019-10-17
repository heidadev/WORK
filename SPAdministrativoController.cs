using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Negocio;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace Web_Gestion_RRHH_SCC.Controllers
{

    public class SPAdministrativoController : Controller
    {
        [Filters.Filtro]
        // GET: SPAdministrativo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Lista_cargo_administrativo(int codigo_centro_costo)
        {
            CargoNG NCargoNG = new CargoNG();

            try
            {
                var lista_cargo = NCargoNG.listar_cargo_administrativo(codigo_centro_costo);
                return new JsonResult { Data = lista_cargo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Lista_area_administrativo(int cod_usuario)
        {
            AreaNG NAreaNG = new AreaNG();

            try
            {
                var lista_area = NAreaNG.listar_area(cod_usuario);
                return new JsonResult { Data = lista_area, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Listar_centro_costo_por_usuario(int cod_area)
        {
            Centro_CostoNG NCentro_CostoNG = new Centro_CostoNG();

            try
            {
                var lista_centro_costo = NCentro_CostoNG.Listar_centro_costo_por_usuario(cod_area);
                return new JsonResult { Data = lista_centro_costo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Listar_centro_costo_por_codigo(int cod_centro_costo)
        {
            Centro_CostoNG NCentro_CostoNG = new Centro_CostoNG();

            try
            {
                var lista_centro_costo = NCentro_CostoNG.Listar_centro_costo_por_codigo(cod_centro_costo);
                return new JsonResult { Data = lista_centro_costo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Obtener_usuario_aprueba_solicitud(int codigo_solicitud_personal_administrativo, int codigo_cargo_usuario_valida, int estado)
        {
            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();

            try
            {
                var objUsuario = NSPAdministrativoNG.Obtener_usuario_aprueba_solicitud(codigo_solicitud_personal_administrativo, codigo_cargo_usuario_valida, estado);
                return new JsonResult { Data = objUsuario, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Validar_nro_requerimiento_registrado(string numero_rq)
        {
            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();

            try
            {
                var cantidad_solicitudRQ = NSPAdministrativoNG.Validar_nro_requerimiento_registrado(numero_rq);
                return new JsonResult { Data = cantidad_solicitudRQ, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Insertar_solicitud_personal_administrativo(HttpPostedFileBase file, string cadenaObjSPAdministrativo, string correo_usuario_asigando, string nombre_usuario_asignado, string fechaIngreso)
        {
            var nivel_error = 0;

            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();
            var numero_RQ_AG = 0;
            byte[] archivo = null;

            Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));

            try
            {
                nivel_error = 1;

                string fecha_ingreso = "";

                string[] _arreglo_fecha_ingreso = fechaIngreso.Split(new char[] { '/' });

                DateTime fecha = new DateTime(Convert.ToInt32(_arreglo_fecha_ingreso[2]), Convert.ToInt32(_arreglo_fecha_ingreso[1]), Convert.ToInt32(_arreglo_fecha_ingreso[0]));
                Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));

                nivel_error = 2;
                fecha_ingreso = fecha.ToString("yyyyMMdd");

                //***********************
                //var length = file.InputStream.Length; //Length: 103050706

                //if (length > 0)
                //{
                //    using (var binaryReader = new BinaryReader(file.InputStream))
                //    {
                //        archivo = binaryReader.ReadBytes(file.ContentLength);
                //    }
                //}
                //***********************


                //if (file.ContentLength > 0)
                //{
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    archivo = ms.GetBuffer();
                }

                Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));
                //}
                nivel_error = 3;

                Solicitud_Personal_Administrativo objSPAdministrativo = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Solicitud_Personal_Administrativo>(cadenaObjSPAdministrativo);

                Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));

                nivel_error = 4;

                objSPAdministrativo.archivo_sustento = archivo;
                objSPAdministrativo.nombre_archivo = System.IO.Path.GetFileName(file.FileName);

                Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));
                nivel_error = 5;

                numero_RQ_AG = NSPAdministrativoNG.Insertar_solicitud_personal_administrativo(objSPAdministrativo, fecha_ingreso);

                Registrar_Log_Error("Solicitud Personal Administrativo " + "NO HAY PROBLEMA", "NORMAL", 1, nivel_error, archivo, System.IO.Path.GetFileName(file.FileName));

                nivel_error = 6;
                if (numero_RQ_AG != 0)
                {
                    enviar_correo(correo_usuario_asigando, nombre_usuario_asignado, Convert.ToString(numero_RQ_AG));

                }

                nivel_error = 7;
                return new JsonResult { Data = numero_RQ_AG, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                Registrar_Log_Error("Solicitud Personal Administrativo " + ex.Message, ex.StackTrace, 1, 99, archivo, System.IO.Path.GetFileName(file.FileName));
                return new JsonResult { Data = numero_RQ_AG, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public void Registrar_Log_Error(string descripcion_error, string detalle_error, int codigo_usuario, int nivel_error, byte[] archivo, string nombre_archivo)
        {
            SPAdministrativoNG SPAdministrativoNG = new SPAdministrativoNG();
            Log_Error objLog_Error = new Log_Error();
            //Guardar Log de error en base de datos
            objLog_Error.descripcion_log_error = descripcion_error;
            objLog_Error.detalle_log_error = detalle_error;
            objLog_Error.codigo_perfil_usuario = 1;
            objLog_Error.codigo_usuario = codigo_usuario;
            objLog_Error.codigo_version_aplicativo = "1";
            objLog_Error.maquina = System.Environment.MachineName;
            objLog_Error.fecha_creacion_registro = DateTime.Now;
            objLog_Error.habilitado = true;
            objLog_Error.nombre_formulario_aplicativo = "SPA";
            objLog_Error.nivel_error = nivel_error;
            objLog_Error.archivo_sustento = archivo;
            objLog_Error.nombre_archivo = nombre_archivo;
            SPAdministrativoNG.Registro_Log_Error(objLog_Error);
        }

        private void enviar_correo(string correo_usuario_asigando, string nombre_usuario_asignado, string numero_requerimiento)
        {
            using (var mail = new MailMessage("sistemas_SCC@scc.com.pe", correo_usuario_asigando))
            {
                string body = "<div style='font-family : sans-serif;'>" +

                    "<h3><strong>SISTEMA DE SOLICITUD Y SEGUIMIENTO DE INGRESOS DE PERSONAL</strong> </h3>" +
                    "<h4><strong>Validación de Solicitud de Personal Administrativo</strong></h4>" +
                    "</div>" +
                    "<div style='font-family : sans-serif; font-size : 12px;'>Estimado(a): " + nombre_usuario_asignado + "</div>" +
                      "<br />" +
                    "<div style='font-family : sans-serif; font-size : 12px;'>Tiene una solicitud de personal administrativo para su validación y aprobación. </div>" +
                      "<br />" +
                    "<div style='font-family : sans-serif; font-size : 12px;'>- N° de Requerimiento: <strong> " + numero_requerimiento + "</strong></div>" +
                      "<br />" +
                    //"<div style='font-family : sans-serif; font-size : 12px;'>Para poder restablecer la contraseña haz click en el siguiente enlace:  <a href='http://localhost:1895/OlvidoContrasenia/Restablecer?xd=" + HttpUtility.UrlEncode(querystringSeguro.ToString()) + "'>Cambiar contraseña</a></div>" +
                    "<div style='font-family : sans-serif; font-size : 12px;'>Para ver el detalle de la solicitud, ingresar al sistema haciendo click en el siguiente enlace:  <a href='http://gestionrrhh/ASPAdministrativo/Index'>Aquí</a></div>" +
                    //"<div style='font-family : sans-serif; font-size : 12px;'>Para ver el detalle de la solicutud ingresar al sistema haciendo click en el siguiente enlace:   <a href='http://solicitudpersonal/Login/Index'>Inicia Sesión</a></div>" +

                    "<br />" +
                      "<div style='font-family : sans-serif; font-size : 12px;'>Si Ud. ha recibido este mensaje por error por favor haga caso omiso de este correo.</div>" +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "<div style='font-family : Calibri; font-size : 15px; font-weight:bold; color:#005CFF'>" +
                    "Servicio de Call Center del Perú" +
                    "</div>" +
                    "<div style='font-family : Calibri; font-size : 13px; font-weight:bold; color:#FF3D00'>" +
                    "Tecnología de la Información – SCC" +
                    "</div>" +
                    "<div style='font-family : Calibri; font-size : 12px; font-weight:bold; color:#848484'>" +
                    "Jr. Camaná 678, 3er Piso Lima Perú" +
                    "</div>" +
                    "<div style='font-family : Calibri; font-size : 12px; font-weight:bold; color:#848484'>" +
                    "Teléfono: +511 711-4400" +
                    "</div>";

                mail.Subject = "Aprobación de Solicitud de Personal Administrativo";
                mail.Body = body;
                mail.IsBodyHtml = true;
                var smtp = new SmtpClient();
                smtp.Host = "sccex02.scc.com.pe";
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("sistemas_SCC", "%s1st3m4s_SCC%");
                //smtp.Port = 587;
                smtp.Send(mail);
                //valor = true;
            }
        }

        [HttpGet]
        public ActionResult Listar_solicitud_personal_administrativo(int codigo_usuario_registra, string fechaDesde, string fechaHasta, string codigo_area, string codigo_centro_costo, string codigo_cargo, string estado, string numero_rq)
        {
            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();

            try
            {
                string fecha_desde = "";
                string fecha_hasta = "";

                if (fechaDesde != "" && fechaHasta != "")
                {
                    string[] _arreglo_fecha_desde = fechaDesde.Split(new char[] { '/' });
                    string[] _arreglo_fecha_hasta = fechaHasta.Split(new char[] { '/' });

                    DateTime fecha_inicial = new DateTime(Convert.ToInt32(_arreglo_fecha_desde[2]), Convert.ToInt32(_arreglo_fecha_desde[1]), Convert.ToInt32(_arreglo_fecha_desde[0]));

                    DateTime fecha_final = new DateTime(Convert.ToInt32(_arreglo_fecha_hasta[2]), Convert.ToInt32(_arreglo_fecha_hasta[1]), Convert.ToInt32(_arreglo_fecha_hasta[0]));

                    fecha_desde = fecha_inicial.ToString("yyyyMMdd");
                    fecha_hasta = fecha_final.ToString("yyyyMMdd 23:59:59");
                }

                var lista = NSPAdministrativoNG.Listar_solicitud_personal_operativo(codigo_usuario_registra, fecha_desde, fecha_hasta, codigo_area, codigo_centro_costo, codigo_cargo, estado, numero_rq);
                return new JsonResult
                {
                    Data = lista,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue,
                    ContentType = "application/json"
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Listar_historial_solicitud_personal_administrativo(int codigo_solicitud_personal_administrativo)
        {
            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();

            try
            {
                var lista = NSPAdministrativoNG.Listar_historial_solicitud_personal_administrativo(codigo_solicitud_personal_administrativo);
                return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Actualizar_solicitud_personal_administrativo(HttpPostedFileBase file, string cadena_objSolicitud, string correo_usuario_asigando, string nombre_usuario_asignado, int codigo_estado, bool cambiar_archivo_sustento, string fechaIngreso)
        {
            SPAdministrativoNG NSPAdministrativoNG = new SPAdministrativoNG();

            try
            {

                byte[] archivo = null;

                if (file != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        archivo = ms.GetBuffer();
                    }
                }

                string fecha_ingreso = "";

                string[] _arreglo_fecha_ingreso = fechaIngreso.Split(new char[] { '/' });

                DateTime fecha = new DateTime(Convert.ToInt32(_arreglo_fecha_ingreso[2]), Convert.ToInt32(_arreglo_fecha_ingreso[1]), Convert.ToInt32(_arreglo_fecha_ingreso[0]));

                fecha_ingreso = fecha.ToString("yyyyMMdd");

                Solicitud_Personal_Administrativo objSPAdministrativo = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Solicitud_Personal_Administrativo>(cadena_objSolicitud);

                if (archivo != null)
                {
                    objSPAdministrativo.archivo_sustento = archivo;
                    objSPAdministrativo.nombre_archivo = System.IO.Path.GetFileName(file.FileName);
                }

                var result = NSPAdministrativoNG.Actualizar_solicitud_personal_administrativo(objSPAdministrativo, codigo_estado, cambiar_archivo_sustento, fecha_ingreso);

                if (result == true)
                {
                    enviar_correo(correo_usuario_asigando, nombre_usuario_asignado, objSPAdministrativo.numero_requerimiento);
                }

                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}