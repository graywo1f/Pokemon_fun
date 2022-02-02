<template>
    <div id="Detils">
        <h1>  {{Details.Name}}</h1>
        
        <div class="Pokemondetails">

            
            <img v-if="showimg" v-bind:src="Details.sprites.frontDefault" />
            <img v-if="showimg" v-bind:src="Details.sprites.frontShiny" />
            <img v-if="showimg" v-bind:src="Details.sprites.backDefault" />
            <img v-if="showimg" v-bind:src="Details.sprites.backShiny" />
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


                    <tr v-for="prop in DetailsColumns.slice(0, 5)">
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
                DetailsColumns: [],
                showimg: false,

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

                        this.Details = response.data.responce;
                        this.DetailsColumns = Object.keys(response.data.responce);
                        this.showimg = true;
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