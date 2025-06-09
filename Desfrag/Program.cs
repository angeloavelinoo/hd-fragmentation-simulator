const int TAMANHO_HD = 100;

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
    int index = hd.Where(x => x.EstaLivre).Select(hd => hd.Index).FirstOrDefault(); // forma fácil de pegar o último bloco livre.

                                                                                    //int index = 0;

                                                                                    //for(int i = 0; i < TAMANHO_HD; i++)
                                                                                    //{
                                                                                    //    bool livre = hd[i].EstaLivre;

                                                                                    //    if (livre)
                                                                                    //    {
                                                                                    //        index = hd[i].Index;
                                                                                    //        break;
                                                                                    //    }

                                                                                    //} forma mais primitiva e ruim de fazer kkkk ..
                                                                                    // daria para fazer salvando o lastIndex tb que reduziria o tempo de execução. Mas como o objetivo do trabalho não é usar formas primitivas, usei LINQ mesmo.



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
    int index = 0;

    //hd = hd.OrderBy(x => x.BlockId).Select(x => new Block
    //{
    //    BlockId = x.BlockId,
    //    Dados = x.Dados,
    //    EstaLivre = x.EstaLivre,
    //    Index = index++ 
    //}).ToArray();  Forma fácil de ordenar e reindexar os blocos do HD


    Block[] novoHd = new Block[TAMANHO_HD];

    List<string> arquivosVistos = new List<string>();
    int lasIndex = 0;

    for (int i = 0; i < hd.Length; i++)
    {
        string? idArq = hd[i].BlockId;

        if (idArq != null && !arquivosVistos.Contains(idArq))
        {
            for (int j = 0; j < TAMANHO_HD; j++)
            {
                if (hd[j].BlockId == idArq)
                {
                    Block newBlock = new Block
                    {
                        EstaLivre = false,
                        Dados = hd[j].Dados,
                        BlockId = idArq,
                        Index = lasIndex
                    };
                    novoHd[lasIndex] = newBlock;
                    lasIndex += 1;
                }
            }

            arquivosVistos.Add(idArq);
        }
    }
}


class Block
{
    public bool EstaLivre { get; set; }
    public string? Dados { get; set; }
    public string? BlockId { get; set; }
    public int Index { get; set; }
}

