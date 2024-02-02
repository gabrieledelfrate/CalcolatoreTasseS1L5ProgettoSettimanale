namespace CalcolatoreTasseS1L5
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal class Program
    {
        static List<Contribuente> contribuenti = new List<Contribuente>();
        static void Main(string[] args)
        {
            bool continua = true;

            while (continua)
            {
                Console.WriteLine("CALCOLATORE TASSE");
                Console.WriteLine("1. Inserisci dati contribuente");
                Console.WriteLine("2. Visualizza storico imposte calcolate");
                Console.WriteLine("3. Esci");

                Console.Write("Seleziona un opzione: ");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        InserisciDatiContribuente();
                        break;
                    case "2":
                        VisualizzaStoricoImposte();
                        break;
                    case "3":
                        continua = false;
                        break;
                    default:
                        Console.WriteLine("Scelta non valida. Riprova.");
                        break;
                }
            }
        }

        static void InserisciDatiContribuente()
        {
            try
            {
                Console.WriteLine("\nInserisci i dati del contribuente:");

                Console.Write("Nome: ");
                string nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Console.ReadLine());

                Console.Write("Cognome: ");
                string cognome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Console.ReadLine());

                DateTime dataNascita;
                Console.Write("Data di Nascita (formato: dd/MM/yyyy): ");

                while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascita)
                || dataNascita.Day > 31 || dataNascita.Month > 12 || dataNascita.Year < 1890)
                {
                    Console.WriteLine("Data non valida. Riprova.");
                    Console.Write("Data di Nascita (formato: dd/MM/yyyy): ");
                }


                Console.Write("Codice Fiscale: ");
                string codiceFiscale = Console.ReadLine().ToUpper();

                Console.Write("Sesso (M/F): ");
                char sesso = char.Parse(Console.ReadLine().ToUpper());

                Console.Write("Comune di Residenza: ");
                string comuneResidenza = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Console.ReadLine());

                Console.Write("Reddito Annuale: ");
                decimal redditoAnnuale = decimal.Parse(Console.ReadLine());

                if (redditoAnnuale <= 0)
                {
                    throw new ArgumentException("Il reddito annuale deve essere maggiore di zero.");
                }

                Contribuente contribuente = new Contribuente(nome, cognome, dataNascita, codiceFiscale, sesso, comuneResidenza, redditoAnnuale);
                contribuenti.Add(contribuente);

                decimal imposta = contribuente.CalcolaImposta();
                Console.WriteLine($"\nIMPOSTA DA PAGARE: € {imposta}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        static void VisualizzaStoricoImposte()
        {
            if (contribuenti.Count == 0)
            {
                Console.WriteLine("Nessun contribuente presente nello storico.");
            }
            else
            {
                Console.WriteLine("\nStorico imposte calcolate:");
                foreach (var contribuente in contribuenti)
                {
                    Console.WriteLine($"Contribuente: {contribuente.Nome} {contribuente.Cognome}, " +
                                      $"nato il {contribuente.DataNascita.ToString("dd/MM/yyyy")} " +
                                      $"({contribuente.Sesso}), residente in {contribuente.ComuneResidenza}, " +
                                      $"codice fiscale: {contribuente.CodiceFiscale}");
                    Console.WriteLine($"Reddito dichiarato: € {contribuente.RedditoAnnuale}");
                    Console.WriteLine($"IMPOSTA DA VERSARE: € {contribuente.CalcolaImposta()}\n");
                }
            }
        }
    }

    class Contribuente
    {
        public string Nome { get; }
        public string Cognome { get; }
        public DateTime DataNascita { get; }
        public string CodiceFiscale { get; }
        public char Sesso { get; }
        public string ComuneResidenza { get; }
        public decimal RedditoAnnuale { get; }

        public Contribuente(string nome, string cognome, DateTime dataNascita, string codiceFiscale, char sesso, string comuneResidenza, decimal redditoAnnuale)
        {
            {
                Nome = nome;
                Cognome = cognome;
                DataNascita = dataNascita;
                CodiceFiscale = codiceFiscale;
                Sesso = sesso;
                ComuneResidenza = comuneResidenza;
                RedditoAnnuale = redditoAnnuale;
            }
            if (!IsValidCodiceFiscale(codiceFiscale))
            {
                throw new ArgumentException("Il formato del codice fiscale è errato.");
            }
        }

        private bool IsValidCodiceFiscale(string codiceFiscale)
        {
            if (codiceFiscale.Length != 16)
            {
                return false;
            }

            if (!char.IsLetter(codiceFiscale[0]) || !char.IsLetter(codiceFiscale[1]) || !char.IsLetter(codiceFiscale[2]) ||
                !char.IsDigit(codiceFiscale[6]) || !char.IsDigit(codiceFiscale[7]) ||
                !char.IsLetter(codiceFiscale[8]) ||
                !char.IsDigit(codiceFiscale[9]) || !char.IsDigit(codiceFiscale[10]) ||
                !char.IsLetter(codiceFiscale[11]) ||
                !char.IsDigit(codiceFiscale[12]) || !char.IsDigit(codiceFiscale[13]) || !char.IsDigit(codiceFiscale[14]))
            {
                return false;
            }
            return true;
        }

        public decimal CalcolaImposta()
        {
            decimal imposta = 0;

            if (RedditoAnnuale <= 15000)
            {
                imposta = RedditoAnnuale * 0.23m;
            }
            else if (RedditoAnnuale <= 28000)
            {
                imposta = 3450 + (RedditoAnnuale - 15000) * 0.27m;
            }
            else if (RedditoAnnuale <= 55000)
            {
                imposta = 6960 + (RedditoAnnuale - 28000) * 0.38m;
            }
            else if (RedditoAnnuale <= 75000)
            {
                imposta = 17220 + (RedditoAnnuale - 55000) * 0.41m;
            }
            else
            {
                imposta = 25420 + (RedditoAnnuale - 75000) * 0.43m;
            }

            return imposta;
        }
    }
}
