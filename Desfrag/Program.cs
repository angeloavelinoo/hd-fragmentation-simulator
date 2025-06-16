const int TAMANHO_HD = 100;
const int TAMANHO_BUFFER_RAM = 10;

Dictionary<string, string> dicio = new()
{
    { "AQ1", "conteudo1" },
    { "AQ2", "conteudo2" },
    { "AQ3", "conteudo3" },
    { "AQ4", "conteudo4" },
    { "AQ5", "conteudo5" },
    { "AQ6", "conteudo6" },
    { "AQ7", "conteudo7" },
    { "AQ8", "conteudo8" },
    { "AQ9", "conteudo9" },
    { "AQ10", "conteudo10" }
};

Block[] hd = new Block[TAMANHO_HD];

for (int i = 0; i < hd.Length; i++)
{
    hd[i] = new Block
    {
        EstaLivre = true,
        Dados = null,
        BlockId = null,
        Index = i
    };
}


for(int i = 0; i < 100; i++)
{

    int random = new Random().Next(1, 10);
    int tamanho = new Random().Next(1, 4);

    string idArq = dicio.Keys.ElementAt(random);
    string conteudo = dicio[idArq];
    Fragmentacao(idArq, conteudo, tamanho);
}

Desfragmentar();

Console.WriteLine("Desfragmentação concluída! Blocos do HD: \n");

void Fragmentacao(string id, string conteudo, int tamanho)
{
    int index = hd.Where(x => x.EstaLivre).Select(hd => hd.Index).FirstOrDefault();
                                                                                  
    if (index == 100 || index + tamanho > 100)
    {
        Console.WriteLine($"O HD possui {100 - index} de armazenamento, e o arquivo enviado possui {tamanho} não é possível inserir!");
        return;
    }

    while (tamanho > 0)
    {
        hd[index].EstaLivre = false;
        hd[index].Dados = conteudo;
        hd[index].BlockId = id;
        tamanho--;
    }
}

void Desfragmentar()
{
    List<Block> ramBuffer = new List<Block>();
    List<Block> blocosOrdenados = new List<Block>();

    for (int i = 0; i < hd.Length; i++)
    {
        if (!hd[i].EstaLivre)
        {
            ramBuffer.Add(hd[i]);

            if (ramBuffer.Count >= TAMANHO_BUFFER_RAM)
            {
                blocosOrdenados.AddRange(ramBuffer.OrderBy(b => b.BlockId));
                ramBuffer.Clear();
            }
        }
    }

    if (ramBuffer.Count > 0)
    {
        blocosOrdenados.AddRange(ramBuffer.OrderBy(b => b.BlockId));
    }

    for (int i = 0; i < hd.Length; i++)
    {
        hd[i].EstaLivre = true;
        hd[i].Dados = null;
        hd[i].BlockId = null;
    }

    int currentIndex = 0;
    foreach (var block in blocosOrdenados.OrderBy(b => b.BlockId))
    {
        if (currentIndex >= TAMANHO_HD)
        {
            Console.WriteLine("[ERRO] HD sem espaço após desfragmentação!");
            break;
        }

        hd[currentIndex].EstaLivre = false;
        hd[currentIndex].Dados = block.Dados;
        hd[currentIndex].BlockId = block.BlockId;
        currentIndex++;
    }

    Console.WriteLine($"Desfragmentação concluída! Blocos ocupados: {blocosOrdenados.Count}");
}


class Block
{
    public bool EstaLivre { get; set; }
    public string? Dados { get; set; }
    public string? BlockId { get; set; }
    public int Index { get; set; }
}

