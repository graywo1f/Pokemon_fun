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
                            <td>
                                {{pokemon.name}}
                            </td>                           
                        </tr>
                    </tbody>
                </table>
                <!--<nav aria-label="..." v-if="allPages > 1">
                    <ul class="pagination">
                        <li class="page-item" v-for="(page, index) in allPages" :key="index">
                            <a class="page-link" v-if="!IsCurrentPage(index)" v-on:click="changePage(index + 1)">{{ index+1 }}</a>
                            <a class="page-link" v-if="IsCurrentPage(index)" v-on:click="changePage(index + 1)">{{ index+1 }}</a>
                        </li>
                    </ul>
                </nav>-->
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
               
                currentPage: 1,
                allPages: 1,
                totalItems: 0,
                
                loading: true,
               

            }
        },
       
       
        methods: {
            //getPropValue: function ($this, fieldName) {
            //    if ($this.hasOwnProperty(fieldName)) {
            //        if (Array.isArray($this[fieldName])) {
            //            return $this[fieldName].join(" , ");
            //        }
            //        else {
            //            return $this[fieldName];
            //        }
            //    }
            //    return
            //},
            IsCurrentPage(index) {
                if (index == this.currentPage)
                    return true;
                else return false;
            },
            changePage(page) {
                this.currentPage = page;
                this.UpdateData();

            },           
           
           
            UpdateData() {
                this.loading = true;

                axios.get('/API/Pokemon')
                    .then(response => {

                        this.Pokemons = response.data.results;
                        //this.allPages = response.data.table.totalPages;
                        this.totalItems = response.data.count;
                        //this.currentPage = response.data.table.currentPage;

                        this.loading = false;
                        this.hideSearch = false;
                    })
            }
        },

        computed: {

        },
        created() {
            this.UpdateData();

        },
      
    };
</script>