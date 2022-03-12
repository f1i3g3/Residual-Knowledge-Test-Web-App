using System;
using System.Runtime.Serialization;

namespace CurriculumParser
{
    /// <summary>
    /// Представляет ошибки, которые возникли во время парсинга файла с учебным планом
    /// </summary>
    [Serializable]
    internal class CurriculumParsingException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземляр класса <name>CurriculumParsingException</name>
        /// </summary>
        public CurriculumParsingException()
        {
        }

        /// <summary>
        /// Инициализирует новый экземляр класса <name>CurriculumParsingException</name> 
        /// с соббщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public CurriculumParsingException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Инициализирует новый экземляр класса <name>CurriculumParsingException</name> 
        /// с сообщением об ошибке и исключением, вызвавшим данное
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="innerException">Исключение, вызвавшее данное исключение</param>
        public CurriculumParsingException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Инициализирует новый экземляр класса <name>CurriculumParsingException</name> c сериализованными данными
        /// </summary>
        /// <param name="info">Содержит данные сериализованного объекта об исключении, которое было брошено</param>
        /// <param name="context">Содежит информацию об источнике или назначении</param>
        protected CurriculumParsingException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}