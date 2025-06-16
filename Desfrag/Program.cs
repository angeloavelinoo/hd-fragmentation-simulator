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


for (int i = 0; i < 100; i++)
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
    int posicaoAtual = 0;

    while (posicaoAtual < TAMANHO_HD)
    {
        if (hd[posicaoAtual].EstaLivre)
        {
            posicaoAtual++;
            continue;
        }

        string idAtual = hd[posicaoAtual].BlockId;
        int totalBlocosArquivo = ContarBlocos(idAtual);

        bool jaContiguo = true;
        for (int i = posicaoAtual; i < posicaoAtual + totalBlocosArquivo; i++)
        {
            if (i >= TAMANHO_HD || hd[i].BlockId != idAtual)
            {
                jaContiguo = false;
                break;
            }
        }

        if (jaContiguo)
        {
            posicaoAtual += totalBlocosArquivo;
            continue;
        }

        OrganizarArquivo(idAtual, ref posicaoAtual);
    }
}

void OrganizarArquivo(string idArquivo, ref int posicaoInicial)
{
    List<int> posicoesBlocos = new List<int>();
    for (int i = 0; i < TAMANHO_HD; i++)
    {
        if (hd[i].BlockId == idArquivo)
        {
            posicoesBlocos.Add(i);
        }
    }

    int blocosRestantes = posicoesBlocos.Count;
    int posicaoAlvo = posicaoInicial;
    int indicePosicoes = 0;

    while (blocosRestantes > 0)
    {
        int blocosNestaIteracao = Math.Min(TAMANHO_BUFFER_RAM / 2, blocosRestantes);

        List<Block> buffer = new List<Block>();
        List<int> posicoesOriginais = new List<int>();

        for (int i = 0; i < blocosNestaIteracao; i++)
        {
            if (indicePosicoes + i >= posicoesBlocos.Count) break;

            int posOriginal = posicoesBlocos[indicePosicoes + i];
            buffer.Add(hd[posOriginal]);
            posicoesOriginais.Add(posOriginal);
        }

        for (int i = 0; i < buffer.Count; i++)
        {
            int posDesejada = posicaoAlvo + i;

            if (posicoesOriginais[i] == posDesejada) continue;

            if (!hd[posDesejada].EstaLivre && hd[posDesejada].BlockId != idArquivo)
            {
                int posTemp = EncontrarPosicaoLivre();
                if (posTemp != -1)
                {
                    TrocarBlocos(posDesejada, posTemp);
                }
            }

            TrocarBlocos(posicoesOriginais[i], posDesejada);
        }

        indicePosicoes += buffer.Count;
        blocosRestantes -= buffer.Count;
        posicaoAlvo += buffer.Count;
    }

    posicaoInicial += posicoesBlocos.Count;
}

int EncontrarPosicaoLivre()
{
    for (int i = 0; i < TAMANHO_HD; i++)
    {
        if (hd[i].EstaLivre) return i;
    }
    return -1;
}

void TrocarBlocos(int index1, int index2)
{
    Block temp = new Block
    {
        EstaLivre = hd[index1].EstaLivre,
        Dados = hd[index1].Dados,
        BlockId = hd[index1].BlockId
    };

    hd[index1].EstaLivre = hd[index2].EstaLivre;
    hd[index1].Dados = hd[index2].Dados;
    hd[index1].BlockId = hd[index2].BlockId;

    hd[index2].EstaLivre = temp.EstaLivre;
    hd[index2].Dados = temp.Dados;
    hd[index2].BlockId = temp.BlockId;
}

int ContarBlocos(string id)
{
    int count = 0;
    for (int i = 0; i < TAMANHO_HD; i++)
    {
        if (hd[i].BlockId == id)
        {
            count++;
        }
    }
    return count;
}

class Block
{
    public bool EstaLivre { get; set; }
    public string? Dados { get; set; }
    public string? BlockId { get; set; }
    public int Index { get; set; }
}

