using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace App.Common.Attributes
{
    /// <summary>
    /// Defined an attribute which is used to mark the depended tasks
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependsOnAttribute : Attribute
    {
        public string TypeConstraint
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="taskType">Type of the task.</param>
        public DependsOnAttribute(Type taskType)
        {
            Check.IsNotNull(taskType, "taskType");
            Type typeConstraint = null;
            if (TypeConstraint != null && TypeConstraint.Trim() != string.Empty)
                typeConstraint=Type.GetType(TypeConstraint, true, true);
            if (typeConstraint!=null && !typeConstraint.IsAssignableFrom(taskType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, AppCommon.IncorrectTypeMustBeDescended, typeConstraint.FullName), "taskType");
            }

            TaskType = taskType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        public DependsOnAttribute(string typeName)
            : this(Type.GetType(typeName, true, true))
        {
        }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public Type TaskType { get; private set; }
    }
}
