using ProyectoRRC.Backend.Modelo;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Persona
    /// </summary>
    public class PersonaServicio:ServicioGenerico<Persona>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        private string hash = "$C0ntras3ña_";

        /// <summary>
        /// Metodo donde almacenamos el usuario que quiere iniciar sesion
        /// </summary>
        public Persona usuLogin { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">ContextoContexto de la base de datos</param>
        public PersonaServicio(IncidenciaspartesrrcContext context): base(context)
        {
            contexto = context;
        }
    
        /// <summary>
        /// Metodo que comprueba si el usuario y la contraseña estan en la base de datos
        /// </summary>
        /// <param name="user">usuario</param>
        /// <param name="password">contraseña</param>
        /// <returns>True / False</returns>
        public Boolean login(String user, String password)
        {
            Boolean correcto = true;

            try
            {
                usuLogin = contexto.Set<Persona>().Where(u => u.Dni == user).FirstOrDefault();
            }catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }

            if(usuLogin == null)
            {
                correcto = false;
            }
            else if(!usuLogin.Dni.Equals(user) || !ComprobarContrasenya(password, usuLogin.Contrasenya))
            {
                correcto = false;
            }

            

            return correcto;
        }

        /// <summary>
        /// Metodo que recorre todas las tablas de una persona y almacena en una lista los DNI
        /// </summary>
        /// <returns>Devuelve una lista con todos los DNI de las personas</returns>
        public List<string> obtenerDni()
        {
            List<string> list = new List<string>();

            GetAll.ForEach(x =>
            {
                list.Add(x.Dni);
            });

            return list;
        }
    
        /// <summary>
        /// Metodo que se encarga de encriptar la contraseña 
        /// </summary>
        /// <param name="pwd">Contraseña sin encriptar</param>
        /// <returns></returns>
        public string Encriptar(string pwd)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(pwd);
            using (SHA256 sha256 = SHA256.Create())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    aes.GenerateIV();
                    byte[] iv = aes.IV;

                    ICryptoTransform transform = aes.CreateEncryptor(aes.Key, iv);
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                    byte[] combinedResult = new byte[iv.Length + result.Length];
                    Array.Copy(iv, 0, combinedResult, 0, iv.Length);
                    Array.Copy(result, 0, combinedResult, iv.Length, result.Length);

                    return Convert.ToBase64String(combinedResult);
                }
            }

        }
    
        /// <summary>
        /// Metodo que se encarga de desencriptar la contraseña
        /// </summary>
        /// <param name="pwd">Contraseña encriptada</param>
        /// <returns></returns>
        public string Desencriptar(string pwd)
        {
            byte[] data = Convert.FromBase64String(pwd);

            using (SHA256 sha256 = SHA256.Create())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] ciphertext = new byte[data.Length - iv.Length];
                    Array.Copy(data, 0, iv, 0, iv.Length);
                    Array.Copy(data, iv.Length, ciphertext, 0, ciphertext.Length);

                    aes.IV = iv;

                    ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] result = transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

                    return UTF8Encoding.UTF8.GetString(result);
                }
            }

        }
    
        /// <summary>
        /// Genera una contraseña segura
        /// </summary>
        /// <returns></returns>
        public string GenerarContrasenya()
        {
            Random r = new Random();
            StringBuilder caracteres = new StringBuilder();
            caracteres.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            caracteres.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower());
            caracteres.Append("!@#$%^&*()-_=+[]{}|;:,.<>?");
            caracteres.Append("0123456789");


            return new string(Enumerable.Repeat(caracteres.ToString(), 14).Select(s => s[r.Next(s.Length)]).ToArray());
        }
    
        /// <summary>
        /// Metodo que comprueba si la contraseña es correcta o no
        /// </summary>
        /// <param name="pwdEscrita">Contraseña escrita en la interfaz</param>
        /// <param name="pwdBD">Contraseña almacenada en la base de datos</param>
        /// <returns>True si la contraseña es correcta / False si no lo es</returns>
        public Boolean ComprobarContrasenya(string pwdEscrita, string pwdBD)
        {
            Boolean correcto = false;
            if(Desencriptar(pwdBD).Equals(pwdEscrita)) 
            {
                correcto = true;
            }

            return correcto;
        }
        

    }
}
