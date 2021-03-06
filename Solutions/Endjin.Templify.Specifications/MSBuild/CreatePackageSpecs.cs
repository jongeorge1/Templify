#region License

//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

#endregion

namespace Endjin.Templify.Specifications.MSBuild
{
    using System;

    using Endjin.Templify.Domain.Contracts.Tasks;
    using Endjin.Templify.Domain.Domain.Packages;
    using Endjin.Templify.Domain.Infrastructure;
    using Endjin.Templify.MSBuild;
    using Endjin.Templify.MSBuild.Contracts.Mappers;

    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;

    using Microsoft.Build.Framework;

    using Rhino.Mocks;

    public abstract class specification_for_create_package_msbuild_task : Specification<CreatePackage>
    {
        protected static IPackageCreatorTasks the_package_creator_tasks;

        protected static ICommandOptionsMapper the_command_options_mapper;

        Establish context = () =>
            {
                the_package_creator_tasks = DependencyOf<IPackageCreatorTasks>();
                the_command_options_mapper = DependencyOf<ICommandOptionsMapper>();
            };
    }

    [Subject(typeof(CreatePackage))]
    public class when_the_create_package_msbuild_task_is_executed : specification_for_create_package_msbuild_task
    {
        static CommandOptions the_command_options;

        static bool result;
        
        Establish context = () =>
            {
                the_command_options = new CommandOptions();
                the_command_options_mapper.Stub(m => m.MapFrom(subject)).Return(the_command_options);
            };

        Because of = () => result = subject.Execute();

        It should_ask_the_command_options_mapper_to_map_from_itself = () => the_command_options_mapper.AssertWasCalled(m => m.MapFrom(subject));

        It should_ask_the_package_creator_tasks_to_create_the_package = () => the_package_creator_tasks.AssertWasCalled(t => t.CreatePackage(the_command_options));

        It should_return_true = () => result.ShouldBeTrue();
    }

    [Subject(typeof(CreatePackage))]
    public class when_the_create_package_msbuild_task_is_executed_and_the_package_creation_fails : specification_for_create_package_msbuild_task
    {
        static bool result;

        static Exception the_package_creator_tasks_exception;

        static IBuildEngine the_build_engine;

        Establish context = () =>
        {
            the_package_creator_tasks_exception = new Exception();
            the_package_creator_tasks.Stub(t => t.CreatePackage(Arg<CommandOptions>.Is.Anything)).Throw(
                the_package_creator_tasks_exception);

            the_build_engine = An<IBuildEngine>();
            subject.BuildEngine = the_build_engine;
        };

        Because of = () => result = subject.Execute();

        It should_log_the_error_details = () => the_build_engine.AssertWasCalled(b => b.LogErrorEvent(Arg<BuildErrorEventArgs>.Is.Anything));

        It should_return_false = () => result.ShouldBeFalse();
    }

    [Subject(typeof(CreatePackage))]
    public class when_the_create_package_msbuild_task_receives_a_progress_update_from_the_package_creation_tasks : specification_for_create_package_msbuild_task
    {
        static bool result;

        static PackageProgressEventArgs the_progress_event_args;

        static IBuildEngine the_build_engine;

        Establish context = () =>
        {
            the_build_engine = An<IBuildEngine>();
            subject.BuildEngine = the_build_engine;
            the_progress_event_args = new PackageProgressEventArgs(ProgressStage.BuildPackage, 100, 10); 
        };

        Because of = () => the_package_creator_tasks.Raise(x => x.Progress += null, the_package_creator_tasks, the_progress_event_args);

        It should_log_a_message_containing_the_progress_details = () => the_build_engine.AssertWasCalled(e => e.LogMessageEvent(Arg<BuildMessageEventArgs>.Is.Anything));
    }
}