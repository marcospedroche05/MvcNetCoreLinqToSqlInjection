using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region STORED PROCEDURES

//create or replace procedure SP_DELETE_DOCTOR
//(p_iddoctor DOCTOR.DOCTOR_NO%type)
//AS
//begin
//    delete from DOCTOR where DOCTOR_NO=p_iddoctor;
//commit;
//end;

#endregion

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctor;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/FREEPDB1; Persist Security Info=true;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            this.tablaDoctor = new DataTable();
            string sql = "SELECT * FROM DOCTOR";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            ad.Fill(this.tablaDoctor);
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doc);
            }
            return doctores;
        }

        public async Task CreateDoctorAsync(int idDoctor, string apellido, string especialidad,
                                int salario, int idHospital)
        {
            string sql = "INSERT INTO DOCTOR VALUES(:idhospital, :id, :apellido, :especialidad, :salario)";

            //AQUI VAN LOS PARAMETROS...
            OracleParameter pamIdHospital = new OracleParameter(":idhospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":id", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamEspe = new OracleParameter(":especialidad", especialidad);
            OracleParameter pamSal = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamIdHospital);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspe);
            this.com.Parameters.Add(pamSal);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();

            this.com.Parameters.Clear();
        }

        public async Task DeleteDoctorAsync(int idDoctor)
        {
            string sql = "SP_DELETE_DOCTOR";
            OracleParameter pamId = new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();

            this.com.Parameters.Clear();
        }

        public async Task UpdateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string sql = "SP_UPDATE_DOCTOR";
            OracleParameter pamIdHospital = new OracleParameter(":p_idhospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":p_iddoctor", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":p_apellido", apellido);
            OracleParameter pamEspe = new OracleParameter(":p_especialidad", especialidad);
            OracleParameter pamSal = new OracleParameter(":p_salario", salario);
            this.com.Parameters.Add(pamIdHospital);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspe);
            this.com.Parameters.Add(pamSal);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();

            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == idDoctor
                           select datos;
            var row = consulta.First();
            Doctor doc = new Doctor
            {
                IdHospital = row.Field<int>("HOSPITAL_COD"),
                IdDoctor = row.Field<int>("DOCTOR_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Especialidad = row.Field<string>("ESPECIALIDAD"),
                Salario = row.Field<int>("SALARIO")
            };
            return doc;
        }

        public List<Doctor> GetDoctoresByEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable()
                           where datos.Field<string>("ESPECIALIDAD") == especialidad
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO")
                };
                doctores.Add(doc);
            }
            return doctores;
        }
    }
}
