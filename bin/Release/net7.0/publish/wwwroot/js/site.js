let CategoriaSelect = document.querySelector('#CategoriaSelect')
let Categorias = document.querySelectorAll(".categoria")
let BarraPesquisa = document.querySelector('.BarraPesquisa')

if (CategoriaSelect) {
    CategoriaSelect.addEventListener('change', SetCategoria)
    SetCategoria()
}


function SetCategoria() {
    Categorias.forEach(item => {
        var nome = item.querySelector('.Nome').innerHTML

        if (nome === CategoriaSelect.value) {
            item.style.display = "block"
        } else {
            item.style.display = "none"
        }

    })
}

if (BarraPesquisa) {
    BarraPesquisa.addEventListener('keyup', (e) => {
        if (e.key === 'Enter' || e.keyCode === 13) {
            Categorias.forEach(item => {
                var cards = item.querySelectorAll('.card')

                cards.forEach(card => {

                    var nome = card.querySelector('.card-title').innerHTML.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "")
                    var filtro = BarraPesquisa.value.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "")

                    if (nome.toLowerCase().indexOf(filtro) > -1) {
                        var Card = card.parentNode.parentNode
                        Card.scrollIntoView()
                        card.style.border = "2px solid var(--Laranja)";
                    } else {
                        card.style.border = "none";
                    }

                })
            })
        }

    })
}

//Obtendo o valor do parâmetro de qual botão foi clicado
var parametros = new URLSearchParams(window.location.search);
var acaoDoBotao = parametros.get("botao");
if (acaoDoBotao == "Relatorio") {
    window.print();
}