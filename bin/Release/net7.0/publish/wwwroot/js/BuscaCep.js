let CEP = document.querySelector('#Cep')
let Endereco = document.querySelector('#Endereco')

consultaCep()

async function consultaCep() {
    //! Limpando cmapos 

    Endereco.value = ""

    //$Armazenando valor do CEP

    var cepformatado = CEP.value.split('.').join("").split('-').join("");
    var valor = cepformatado

    //# Monta o link ára faser a pesquisa na Api Do ViaCep

    var Url = `https://viacep.com.br/ws/${valor}/json/`

    //! faz uma requisição asincrona(fetch) para url

    await fetch(Url)
        .then(resp => resp.json()) //$ converte a resposta emformato json
        .then(json => {

            Endereco.value = `${json.logradouro} | ${json.bairro} | ${json.localidade} | ${json.uf}`

        })
        .catch(e =>{
            alert(`cep ${valor} errado ou ñ encontrado`)
        })

}