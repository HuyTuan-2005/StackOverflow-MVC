using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using StackOverflow.Repositories;
using StackOverflow.Services;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

// namespace StackOverflow.UnityConfig
// {
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            var connString = WebConfigurationManager.ConnectionStrings["ForumDB"].ConnectionString;

            // Đăng ký ánh xạ interface sang class
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserRepository, UserRepository>(new InjectionConstructor(connString));
            
            container.RegisterType<IQuestionService, QuestionService>();
            container.RegisterType<IQuestionRepository, QuestionRepository>(new InjectionConstructor(connString));
            
            container.RegisterType<IAnswerService, AnswerService>();
            container.RegisterType<IAnswerRepository, AnswerRepository>(new InjectionConstructor(connString));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
// }