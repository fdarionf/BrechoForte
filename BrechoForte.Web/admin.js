// Configuração
const PORTA_API = 7134; // <--- Sua porta correta
const URL_API_PRODUTO = `https://localhost:${PORTA_API}/api/Produto`;
const URL_API_CONFIG = `https://localhost:${PORTA_API}/api/Configuracao`;

let idEmEdicao = null;

// --- 1. LOGIN (MANTIDO IGUAL) ---
async function tentarLogin() {
    const senhaDigitada = document.getElementById('campo-senha').value;
    const msgErro = document.getElementById('msg-erro');
    const botao = document.querySelector('button[onclick="tentarLogin()"]');

    botao.innerText = "Verificando...";
    botao.disabled = true;

    try {
        const resposta = await fetch(`${URL_API_CONFIG}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(senhaDigitada)
        });

        if (resposta.ok) {
            document.getElementById('tela-login').classList.add('hidden');
            document.getElementById('painel-admin').classList.remove('hidden');

            carregarProdutosAdmin();
            carregarConfigAdmin();   // <--- Chama a função atualizada
        } else {
            msgErro.innerText = "Senha incorreta!";
            document.getElementById('campo-senha').value = "";
        }
    } catch (erro) {
        console.error(erro);
        msgErro.innerText = "Erro de conexão com o servidor.";
    } finally {
        botao.innerText = "Entrar";
        botao.disabled = false;
    }
}

function sair() { location.reload(); }

// --- 2. CONFIGURAÇÃO DA LOJA (ATUALIZADO PARA WHATS E SENHA) ---

// Busca os dados para preencher os campos na tela
async function carregarConfigAdmin() {
    try {
        const resposta = await fetch(URL_API_CONFIG);
        const config = await resposta.json();

        // Preenche o Nome
        document.getElementById('novo-nome-loja').value = config.nomeLoja;

        // Preenche o WhatsApp (se existir no banco)
        if (config.whatsapp) {
            document.getElementById('novo-whatsapp').value = config.whatsapp;
        }

        document.getElementById('novo-email').value = config.emailContato || "";

    } catch (e) { console.error("Erro ao buscar config", e); }
}

// Salva TUDO (Nome, Whats e Senha) - Essa é a nova função que o botão chama
async function salvarConfiguracoes() {
    const novoNome = document.getElementById('novo-nome-loja').value;
    const novoWhats = document.getElementById('novo-whatsapp').value;
    const novaSenha = document.getElementById('nova-senha').value;
    const novoEmail = document.getElementById('novo-email').value;

    if (!novoNome) { alert("O nome da loja é obrigatório!"); return; }

    const dadosAtualizados = {
        nomeLoja: novoNome,
        whatsapp: novoWhats,
        emailContato: novoEmail,
        senhaAdmin: novaSenha // Se estiver vazio, o Backend ignora. Se tiver texto, ele troca.
    };

    try {
        const resposta = await fetch(URL_API_CONFIG, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dadosAtualizados)
        });

        if (resposta.ok) {
            alert("✅ Configurações salvas com sucesso!");
            if (novaSenha) {
                alert("⚠️ A senha foi alterada! Anote a nova senha.");
            }
        } else {
            alert("Erro ao salvar configuração.");
        }
    } catch (erro) {
        alert("Erro de conexão.");
    }
}

// --- 3. PRODUTOS (MANTIDO IGUAL) ---

async function carregarProdutosAdmin() {
    try {
        const resposta = await fetch(URL_API_PRODUTO);
        const produtos = await resposta.json();

        const tbody = document.getElementById('tabela-produtos');
        tbody.innerHTML = '';

        produtos.forEach(p => {
            const linha = `
                <tr style="border-bottom: 1px solid #eee;">
                    <td style="padding: 10px;">
                        <img src="${p.fotoUrl || 'https://via.placeholder.com/50'}" width="50" height="50" style="object-fit:cover; border-radius:4px;">
                    </td>
                    <td style="padding: 10px;">${p.nome}</td>
                    <td style="padding: 10px;">R$ ${p.preco.toFixed(2)}</td>
                    <td style="padding: 10px; text-align: right;">
                        <button onclick="prepararEdicao(${p.id})" style="background:#ff9800; color:white; padding:5px 10px; margin-right:5px; font-size:0.8rem; border:none; border-radius:3px; cursor:pointer;">✏️</button>
                        <button onclick="deletarProduto(${p.id})" style="background:#f44336; color:white; padding:5px 10px; font-size:0.8rem; border:none; border-radius:3px; cursor:pointer;">🗑️</button>
                    </td>
                </tr>
            `;
            tbody.innerHTML += linha;
        });
    } catch (erro) {
        console.error("Erro ao listar:", erro);
    }
}

async function deletarProduto(id) {
    if (confirm("Tem certeza que quer apagar?")) {
        try {
            await fetch(`${URL_API_PRODUTO}/${id}`, { method: 'DELETE' });
            carregarProdutosAdmin();
        } catch (erro) { alert("Erro ao deletar."); }
    }
}

async function prepararEdicao(id) {
    const resposta = await fetch(`${URL_API_PRODUTO}/${id}`);
    const produto = await resposta.json();

    document.getElementById('nome').value = produto.nome;
    document.getElementById('preco').value = produto.preco;
    document.getElementById('descricao').value = produto.descricao;
    document.getElementById('fotoUrl').value = produto.fotoUrl;
    document.getElementById('tamanho').value = produto.tamanho;

    idEmEdicao = id;
    const btn = document.querySelector('.btn-salvar');
    btn.innerText = "🔄 Atualizar Produto";
    btn.style.backgroundColor = "#ff9800";
}

async function salvarProduto() {
    const nome = document.getElementById('nome').value;
    const preco = document.getElementById('preco').value;
    const descricao = document.getElementById('descricao').value;
    const fotoUrl = document.getElementById('fotoUrl').value;
    const tamanho = document.getElementById('tamanho').value;

    if (!nome || !preco || !tamanho) {
        alert("Preencha Nome, Preço e Tamanho!");
        return;
    }

    let precoCorrigido = preco ? parseFloat(preco.replace(',', '.')) : 0;
    const urlFinal = fotoUrl && fotoUrl.trim() !== "" ? fotoUrl : null;

    const objetoProduto = {
        nome: nome,
        descricao: descricao,
        preco: precoCorrigido,
        fotoUrl: urlFinal,
        tamanho: tamanho,
        estoque: 1,
        dataCadastro: new Date().toISOString()
    };

    if (idEmEdicao) {
        objetoProduto.id = idEmEdicao;
    }

    try {
        const resposta = await fetch(idEmEdicao ? `${URL_API_PRODUTO}/${idEmEdicao}` : URL_API_PRODUTO, {
            method: idEmEdicao ? 'PUT' : 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(objetoProduto)
        });

        if (resposta.ok) {
            alert("Sucesso! ✅");
            limparFormulario();
            carregarProdutosAdmin();
        } else {
            const texto = await resposta.text();
            alert("Erro: " + texto);
        }
    } catch (erro) {
        console.error(erro);
        alert("Erro na conexão");
    }
}

function limparFormulario() {
    document.getElementById('nome').value = "";
    document.getElementById('preco').value = "";
    document.getElementById('descricao').value = "";
    document.getElementById('fotoUrl').value = "";
    document.getElementById('tamanho').value = "";

    idEmEdicao = null;
    const btn = document.querySelector('.btn-salvar');
    btn.innerText = "💾 Cadastrar Produto";
    btn.style.backgroundColor = "#2e7d32";
}