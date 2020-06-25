using NotificationService.Model;

namespace NotificationService.Sender
{
    /// <summary>
    /// BaseMessageSender Send methodunu dışarıya açmak için oluşturuldu.
    /// BaseMessageSender tipinde obje oluşturabilmek için result tipini bilmeye gerek varken
    /// bu interfaceden türemmiş bir instance için sadece mesaj tipini bilmek yeterli
    /// Manager da elimizde sadece gönderilecek mesaj tipi var. BU interface ile result tipini bilmeye gerek kalmıyor.
    /// </summary>
    /// <typeparam name="TMes">Mesaj tippi</typeparam>
    public interface IMessageSender<TMes> where TMes : BaseMessage
    {
        int Send(TMes mes);
    }
}
