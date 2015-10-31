using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Delegados
{
    public partial class etHora : Form
    {
        //crear el objeto del hilo secundario
        private Thread hiloSecundario;
        //definir como miembro de la actual forma un delegado que habilite las llamadas asincronas para acceder a la propiedad
        //Value del progressBar
        //Cada delegado debe ser del tipo de operacion que va a ejecutar
        //value = int double etc         text = string,etc...
        private delegate void setValueDelegate(int prValue);
        private void setValue_progressBar(int hecho)
        {   //si el progressBar necesita ser invocado ya que pudo ser llamado desde otro hilo
            if (progressBar1.InvokeRequired)
            {//acceso seguro a la propiedad value del progressBar
                setValueDelegate delegado = new setValueDelegate(setValue_progressBar);
                progressBar1.Invoke(delegado, new object[] { hecho });
            }
            else
            {
                progressBar1.Value = hecho;
            }
        }
        
        
        //constructor
        public etHora()
        {
            InitializeComponent();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            progressBar1.Value = 0;
            //tareaSecundaria();
            //Crear el delegado que se encargará de llamar al hiloSecundario
            ThreadStart delegado = new ThreadStart(tareaSecundaria);
            //ThreadStart es una clase para delegados, su función es llamar al constructor de la clase Thread para iniciar un nuevo hilo
            //Creacion del hilo
            hiloSecundario = new Thread(delegado);
            //ejecución dle hilo
            hiloSecundario.Start();

            //METODOS de los hilos
            //Start = inicia un hilo
            //sleep = detiene un hilo durante un tiempo determinado
            //suspend = interrumpe un hilo cuando alcanza un punto determinado
            //abort = detiene un hilo
            //resume = revive un hilo
            //join = deja en espera un hilo hasta que termina otro diferente. Se puede definir un tiempo de espera.

            //PROPIEDADES DE UN HILO
            //IsAlive = devuelve true si un hilo se encuentra activo
            //IsBackGorund = indicar que un hilo será ejecutado en segundo plano si se pone como true. 
            //name = permite obtener o establecer el nombre de un hilo
            //priority = asigna niveles de prioridad a un hilo
            //ThreadState = describe el estado del hilo.
        }

        private void tareaSecundaria()
        {
            int hecho = 0, tpHecho = 0;
            while (hecho < numericUpDown1.Value)
            {
                hecho += 1;
                tpHecho = (int)(hecho / numericUpDown1.Value * 100);
                if (tpHecho> progressBar1.Value)
                {
                   // progressBar1.Value = tpHecho;
                    //llamada al metodo donde se valora si ocupa ser invocado el delegado o no
                    setValue_progressBar(tpHecho);
                }
            }
           // button1.Enabled = true;
           // numericUpDown1.Enabled = true;
            //habilitar los controles que fueron deabilitados desde otro hilo
            
            if (hiloSecundario.ThreadState == 0)
            {
                Label lblHilo = new Label();
                lblHilo.Text = "Hilo Ejecutandose";
            }

            
            
        }
    }
}
