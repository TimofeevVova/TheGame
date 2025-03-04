using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardGame.Models
{
    public class CardsModel
    {
        // Уникальный идентификатор карты
        public int Id { get; set; }

        // Текст карты
        public required string Text { get; set; }

        // Тип карты
        public required string TypeCard { get; set; }

    }
}
