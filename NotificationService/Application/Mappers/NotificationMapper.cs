using Application.Models;
using Domain.Entity;

namespace Application.Mappers;

public static class NotificationMapper
{
    public static NotificationDto ToDto(Notification entity)
    {
        return new NotificationDto(entity.Id, entity.Message, entity.CreatedAt, entity.IsRead, entity.UserId);
    }

    public static Notification ToEntity(NotificationDto dto)
    {
        return new Notification(dto.Id,  dto.UserId, dto.Message, dto.CreatedAt, dto.IsRead);
    }
}