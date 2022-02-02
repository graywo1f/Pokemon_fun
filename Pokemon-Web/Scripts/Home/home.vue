<template>
    <div id="home">
        <h1>PokemonList</h1>
        
            <div class="PokemonList ">
            
                
                <table class="table contactsTable">
                    <thead>
                        <tr>
                            <th>
                               Name
                            </th>
                            <th >
                              Operation
                            </th>

                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="pokemon in Pokemons">
                            <td> <a v-bind:href="'/Home/Details/'+pokemon.id">
                                {{pokemon.name}}
                                </a>
                            </td>                           
                        </tr>
                    </tbody>
                </table>
                <nav>
                    <a class="page-link" href="#" v-on:click="changePageprv()">Previews</a>
                    <a class="page-link" href="#" v-on:click="changePageNext()">Next</a>
                </nav>
            </div>
       
    </div>

</template>
<script>
    import axios from 'axios'
   
    export default {
        name: "Pokemon-list-component",
     data() {
            return {
                Pokemons: [],
              
                totalItems: 0,
                offsetNext: 0,
                offsetPreviews:0,
                loading: true,
               

            }
        },
       
       
        methods: {
           
           
            changePageNext() {

                this.UpdateData(this.offsetNext);
            },
            changePageprv() {

                this.UpdateData(this.offsetPreviews);
            },
           
           
            UpdateData(offest) {
                if (offest === undefined) {
                    offest = 0;
                }
                axios.get('/API/Pokemon?id=' + offest)
                    .then(response => {

                        this.Pokemons = response.data.responce.results;
                        this.offsetNext = response.data.responce.offsetForNext;
                        this.offsetPreviews = response.data.responce.offsetForPreview;
                        this.totalItems = response.data.responce.count;
                        //this.currentPage = response.data.table.currentPage;

                        this.loading = false;
                        this.hideSearch = false;
                    })
            }
        },

        computed: {

        },
        created() {
            this.UpdateData(0);

        },
      
    };
</script>