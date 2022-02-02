<template>
    <div id="Detils">
        <h1>  {{Details.Name}}</h1>
        
        <div class="Pokemondetails">

            <img v-bind:src="Details.sprites.frontDefault" />
            <img v-bind:src="Details.sprites.frontShiny" />
            <img v-bind:src="Details.sprites.backDefault" />
            <img v-bind:src="Details.sprites.backShiny" />
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            Property Name
                        </th>
                        <th>
                            Property Value
                        </th>

                    </tr>
                </thead>
                <tbody>


                    <tr v-for="prop in DetailsColumns">
                        <td>
                            {{prop}}
                        </td>
                        <td>
                            {{Details[prop]}}
                        </td>
                    </tr>

                </tbody>
            </table>

        </div>
       
    </div>

</template>
<script>
    import axios from 'axios'
   
    export default {
        name: "Pokemon-Details-component",
     data() {
            return {
                Details: [],
               DetailsColumns:[],
               

            }
        },
       
        props: {
            pokemon: String,

        },
        methods: {
         
         
           
           
            UpdateData() {
                this.loading = true;

                axios.get('/API/details/' + this.pokemon)
                    .then(response => {

                        this.Details = response.data;
                        this.DetailsColumns = Object.keys(response.data);
                   
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