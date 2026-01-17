// script.js

// CONFIGURAÇÃO:
const PORTA_API = 7134; // <--- Confira se a porta é essa mesma
const URL_BASE = `https://localhost:${PORTA_API}/api`;
const URL_API = `${URL_BASE}/Produto`;

// Variável Global para o WhatsApp (Começa com um padrão, mas vai mudar)
let whatsappLoja = "5551999999999";

// 1. Carrega Nome da Loja e WhatsApp do Banco de Dados
async function carregarConfiguracao() {
    try {
        const resposta = await fetch(`${URL_BASE}/Configuracao`);
        if (resposta.ok) {
            const config = await resposta.json();

            // Atualiza o Título na página (h1)
            const titulo = document.getElementById('titulo-loja');
            if (titulo) titulo.innerText = config.nomeLoja;

            // Atualiza o nome na aba do navegador
            document.title = config.nomeLoja;

            // Atualiza o WhatsApp da variável global
            if (config.whatsapp) {
                whatsappLoja = config.whatsapp;
            }
        }
    } catch (erro) {
        console.error("Erro ao carregar configurações:", erro);
    }
}

// 2. Carrega os Produtos
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

            // AQUI ESTÁ A MÁGICA:
            // Usamos ${whatsappLoja} no link em vez do número fixo
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
// 1. Carrega a configuração (Nome e Whats)
carregarConfiguracao();
// 2. Carrega os produtos
carregarProdutos();