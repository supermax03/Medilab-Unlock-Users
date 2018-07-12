using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;




namespace UnlockUsersService
{
    public partial class Scheduler : ServiceBase
    {
        private Timer timer1 = null;
        MedicinaEntities2 db;
        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["Time"]);
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            this.timer1.Enabled = true;
            Library.WriteErrorLog("Desbloqueo de usuarios comenzando...");
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Library.WriteErrorLog("Comenzando a actualizar....");
            try
            {

                Library.WriteErrorLog("Desbloqueo de usuarios actualizando...");
                db = new MedicinaEntities2();
                List<Usuario> lista = db.Usuario.Where(s => s.Estado.Equals((int)Helper.EstadosUsuario.Bloqueado)).ToList();
                foreach (Usuario item in lista)
                {

                    item.Estado = (int)Helper.EstadosUsuario.Activo;
                }
                db.SaveChanges();

            }
            catch (Exception exc)
            {
                Library.WriteErrorLog("Error" + exc.InnerException.Message);
                Library.WriteErrorLog("Error" + exc.Message);
            }
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.WriteErrorLog("Desbloqueo de usuarios detenido...");
        }
    }
}
