using System;
using System.Text;
using System.Globalization;
public class test
{
    public static void Main()
    {
        byte[] tavut = asetaTavut(1, 3, 24, 11, 1971, "TIEA322", "jopeuusi", "Hello TIEA322");

        string heksa = BitConverter.ToString(tavut);
        System.Console.WriteLine("Arvoilla versio=1, viesti=3, day=24, month=11, year=1971,\r\n asiakasnimi=TIEA322, usernimi=jopeuusi, teksti=Hello TIEA322,\r\nkoodisi antaa:\r\n {0}", heksa);
    }

    // BYCODEBEGIN
    /// <summary>
    /// Funktio palauttaa tavu-taulukon, missä on Multicastchat protokollan
    /// kehysrakenteen kenttien mukaiset informaatiot
    /// </summary>
    /// <returns>parametreista muodostetut tavut</returns>
    public static byte[] asetaTavut(int versio, int viesti,
                                    int day, int month, int year,
                                    string asiakasnimi,
                                    string usernimi, string teksti)
    {
        // selvitä string-kenttien pituudet UTF-8 tavuina
        byte[] ba1 = Encoding.UTF8.GetBytes(teksti);
        int clientLength = asiakasnimi.Length; // UTF-8 koodattujen tavujen määrä
        int userLength = usernimi.Length; // UTF-8 koodattujen tavujen määrä
        int dataLength = ba1.Length; // UTF-8 koodattujen tavujen määrä
        // Otsikon vakiopituiset kentät on 7 tavua
        int constLength = 7;
        byte[] tavut = new byte[constLength + clientLength + userLength + dataLength];
        // Toteuta
        tavut[0] = Convert.ToByte((versio << 4) & 0x00FF);
        tavut[0] += Convert.ToByte((viesti));
        tavut[1] = Convert.ToByte((day << 3) & 0x00FF);
        tavut[1] += Convert.ToByte((month >> 1) & 0x00FF);
        tavut[2] = Convert.ToByte((month << 7) & 0x00FF);
        tavut[2] += Convert.ToByte((year >> 4) & 0x00FF);
        tavut[3] = Convert.ToByte((year << 4) & 0xFF);
        byte[] ba = Encoding.Default.GetBytes(asiakasnimi);
        var hex = BitConverter.ToString(ba);
        hex = hex.Replace("-", "");
        //string a = hexString.Substring(1, 2);
        tavut[4] = (byte)(clientLength);
        for (int i = 5, j = 0; i < i + clientLength && j < clientLength; i++, j++)
        {
            tavut[i] = (byte)((byte)(ba[j]) & 0xFF);
        }

        ba = Encoding.Default.GetBytes(usernimi);
        hex = BitConverter.ToString(ba);
        hex = hex.Replace("-", "");
        tavut[5 + clientLength] = (byte)(userLength);
        for (int i = 6 + clientLength, j = 0; i < i + userLength && j < userLength; i++, j++)
        {
            tavut[i] = (byte)((byte)(ba[j]) & 0xFF);
        }

        ba = Encoding.UTF8.GetBytes(teksti);
        hex = BitConverter.ToString(ba);
        hex = hex.Replace("-", "");
        int pit = (byte)ba.Length;
        tavut[6 + clientLength + userLength] = (byte)ba.Length;
        for (int i = 7 + clientLength + userLength, j = 0; i < i + dataLength && j < dataLength; i++, j++)
        {
            tavut[i] = (byte)((byte)(ba[j]) & 0xFF);
        }


        System.Console.WriteLine(hex);
        return tavut;
    }
    // BYCODEEND
}

