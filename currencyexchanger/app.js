const URL="https://cat-fact.herokuapp.com/facts"
const factspara=document.querySelector("#fact")

const getFacts = async ()=>{
console.log("getting data");
let response = await fetch (URL);
console.log(response);
let data=await response.json();
factspara.innerText=data[0].text;
}
const btn=document.querySelector("#btn")
btn.addEventListener("click",getFacts)