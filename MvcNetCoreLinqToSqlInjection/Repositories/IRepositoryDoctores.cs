using MvcNetCoreLinqToSqlInjection.Models;

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();
        Doctor FindDoctor(int idDoctor);
        Task CreateDoctorAsync(int idDoctor, string apellido, string especialidad,
                                int salario, int idHospital);
        Task DeleteDoctorAsync(int idDoctor);
        Task UpdateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad,
                                int salario);
        List<Doctor> GetDoctoresByEspecialidad(string especialidad);

    }
}
