// script.js

// CONFIGURAÇÃO:
const PORTA_API = 7134;
const URL_BASE = `https://localhost:${PORTA_API}/api`;
const URL_API = `${URL_BASE}/Produto`;

// Variável Global para o WhatsApp
let whatsappLoja = "5551999999999";

// 1. Carrega Nome da Loja, WhatsApp e AGORA O EMAIL do Banco de Dados
async function carregarConfiguracao() {
    try {
        const resposta = await fetch(`${URL_BASE}/Configuracao`);
        if (resposta.ok) {
            const config = await resposta.json();

            // --- ATUALIZAÇÃO DO HEADER (Já existia) ---
            const titulo = document.getElementById('titulo-loja');
            if (titulo) titulo.innerText = config.nomeLoja;

            document.title = config.nomeLoja;

            if (config.whatsapp) {
                whatsappLoja = config.whatsapp;
            }

            // --- ATUALIZAÇÃO DO FOOTER (Novo) ---

            // 1. Atualiza o nome da loja nos direitos autorais
            const nomeFooter = document.getElementById('nome-loja-footer');
            if (nomeFooter) {
                nomeFooter.innerText = config.nomeLoja;
            }

            // 2. Atualiza o E-mail de Contato
            // IMPORTANTE: Seu C# precisa retornar a propriedade 'email' no JSON para isso funcionar 100%
            const emailFooter = document.getElementById('link-email-footer');
            if (emailFooter) {
                // Se a API trouxe email, usa ele. Se não, usa um padrão ou avisa.
                const emailParaExibir = config.emailContato || "email@padrao.com.br";

                emailFooter.innerText = "✉ " + emailParaExibir;
                emailFooter.href = "mailto:" + emailParaExibir;
            }
        }
    } catch (erro) {
        console.error("Erro ao carregar configurações:", erro);
    }
}

// 2. Carrega os Produtos (Mantido idêntico ao seu original)
async function carregarProdutos() {
    try {
        console.log("1. Buscando produtos...");

        const resposta = await fetch(URL_API);

        if (!resposta.ok) {
            throw new Error('Erro na conexão: ' + resposta.status);
        }

        const produtos = await resposta.json();

        const container = document.getElementById('lista-produtos');
        container.innerHTML = '';

        if (produtos.length === 0) {
            container.innerHTML = '<p>Nenhuma roupa cadastrada no momento.</p>';
            return;
        }

        produtos.forEach(produto => {
            const imagem = produto.fotoUrl ? produto.fotoUrl : 'https://via.placeholder.com/250?text=Sem+Foto';

            const cartaoHTML = `
                <div class="card">
                    <img src="${imagem}" alt="${produto.nome}">
                    <h3>${produto.nome}</h3>
                    <p style="font-weight: bold; color: #555;">Tamanho: ${produto.tamanho}</p>
                    <p>${produto.descricao || ''}</p>
                    <p class="preco">R$ ${produto.preco.toFixed(2)}</p>
                     
                    <a href="https://wa.me/${whatsappLoja}?text=Olá! Tenho interesse no item: ${produto.nome} (Tam: ${produto.tamanho})" 
                       class="btn-whats" target="_blank">
                        🛒 Comprar no Zap
                    </a>
                </div>
            `;

            container.innerHTML += cartaoHTML;
        });

    } catch (erro) {
        console.error(erro);
        document.getElementById('lista-produtos').innerHTML =
            `<p style="color:red; text-align:center">
                😵 Ops! Não consegui carregar as roupas.<br>
                Verifique se a API está rodando.
            </p>`;
    }
}

// Ordem de execução:
carregarConfiguracao();
carregarProdutos();