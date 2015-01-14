using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotifiacionesOfflineSAR {
    public sealed class ManejadorEmail {
        static readonly ManejadorEmail _instance = new ManejadorEmail();
        private static readonly String DESTINATARIO = "correo";
        private static readonly String REMITENTE = "correo";
        private static readonly String PASSWORD_REMITENTE = "password";

        public static ManejadorEmail Instance {
            get {
                return _instance;
            }
        }

        ManejadorEmail() { 
        }

        /**
         * Envía un correo a un destinatario dado, con un asunto y mensaje especificado.
         * @param String destinatario Correo del destinatario.
         * @param String asunto Asunto del correo a enviar.
         * @param String mensaje Cuerpo del correo a enviar.
         */
        public void enviarEmail(String asunto, String mensaje) {
            Boolean enviado = false;

            if (mensaje != "") {
                SmtpClient smtp = configurarSMTP();

                MailMessage correo = prepararCorreoAEnviar(DESTINATARIO, asunto, mensaje);

                while (!enviado) {
                    try {
                        smtp.Send(correo);
                        enviado = true;
                    } catch (Exception) {
                        Console.WriteLine("Error al enviar correo...");
                        Thread.Sleep(300000);
                    }
                }
            }

        }

        /**
         * Prepara el objeto MailMessage a enviar
         * @param String destinatario: email destino.
         * @param String asunti: asunto del email.
         * @param String mensaje: contenido del email.
         * @return MailMessage : objeto a enviar.
         */
        private MailMessage prepararCorreoAEnviar(String destinatario, String asunto, String mensaje) {
            MailMessage correo = crearNuevoCorreo(destinatario);

            añadirContenidoACorreo(asunto, mensaje, correo);

            return correo;
        }

        /**
         * Añade el asunto y contenido a un objeto MailMessage.
         * @param String asunto : asunto del correo a enviar.
         * @param String mensaje : contenido del correo.
         * @MailMessage correo : objeto MailMessage al que se agregan los campos anteriores.
         */
        private void añadirContenidoACorreo(String asunto, String mensaje, MailMessage correo) {
            correo.Subject = asunto;

            correo.Body = mensaje;

            correo.IsBodyHtml = true;
        }

        /**
         * Configura el SMTP a utilizar para enviar el email.
         * @return SmtpClient : SmtpClient configurado.
         */
        private SmtpClient configurarSMTP() {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.ucr.ac.cr";
            smtp.Port = 25;

            smtp.Credentials = new NetworkCredential(REMITENTE, PASSWORD_REMITENTE);
            return smtp;
        }

        /**
         * Crea un nuevo MailMessage con el remitente y destinatario dados.
         * @return MailMessage : objeto con el remitente y destinatario que fueron pasados como parámetro.
         */
        private MailMessage crearNuevoCorreo(String destinatario) {
            MailAddress de = new MailAddress(REMITENTE, "Sistema de Administración de Reactivos");

            MailAddress para = new MailAddress(destinatario);

            MailMessage correo = new MailMessage(de, para);

            return correo;
        }
    }
}
