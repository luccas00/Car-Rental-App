using LocadoraDeVeiculos.Aplicacao.FuncionarioModule;
using LocadoraDeVeiculos.Dominio.FuncionarioModule;
using System;
using System.Threading;
using System.Windows.Forms;

namespace LocadoraDeVeiculos.WindowsApp.Features.Login
{
    public partial class TelaLogin : Form
    {
        private Thread thread;

        private readonly FuncionarioAppService funcionarioAppService;
        private readonly Func<TelaPrincipalForm> telaPrincipalFactory;

        private Funcionario funcionarioLogado;

        public TelaLogin(
            FuncionarioAppService funcionarioAppService,
            Func<TelaPrincipalForm> telaPrincipalFactory)
        {
            InitializeComponent();

            this.funcionarioAppService = funcionarioAppService;
            this.telaPrincipalFactory = telaPrincipalFactory;
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (textUsuario.Text == "admin" && textSenha.Text == "admin")
            {
                funcionarioLogado = new Funcionario("admin", "administrador");
                EfetuarLogin();
                return;
            }

            foreach (Funcionario funcionario in funcionarioAppService.SelecionarTodasEntidade())
            {
                if (textUsuario.Text == funcionario.UsuarioAcesso &&
                    textSenha.Text == funcionario.Senha)
                {
                    funcionarioLogado = funcionario;
                    EfetuarLogin();
                    return;
                }
            }

            textUsuario.Clear();
            textSenha.Clear();
            MessageBox.Show("Invalid Login or Password.");
        }

        private void EfetuarLogin()
        {
            MessageBox.Show($"Welcome {textUsuario.Text}!");

            thread = new Thread(ChamarTelaPrincipal);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            Close();
        }

        public void ChamarTelaPrincipal()
        {
            TelaPrincipalForm.FuncionarioLogado = funcionarioLogado;

            TelaPrincipalForm telaPrincipal = telaPrincipalFactory();

            Application.Run(telaPrincipal);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact the administrator to recreate your password.");
        }
    }
}
