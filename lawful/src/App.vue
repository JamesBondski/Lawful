<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'
import { VBtn, VCard, VCardItem, VCardTitle, VCardActions } from 'vuetify/components'

interface Law {
  Id: number;
  Name: string;
}

interface Section {
  Index: number;
  Title: string;
}

const laws = ref<Law[]>([])
const selectedLawId = ref<number | null>(null)
const sections = ref<Section[]>([])

onMounted(async () => {
  try {
    const response = await axios.get('/api/v1/lawful/law')
    laws.value = response.data
  } catch (error) {
    console.error('Error fetching laws:', error)
  }
})

const fetchSections = async (id: string | number) => {
  try {
    const response = await axios.get(`/api/v1/lawful/section?id=${id}`)
    sections.value = response.data
  } catch (error) {
    console.error('Error fetching sections:', error)
    sections.value = []
  }
}

const storeSection = async (lawId: number, sectionIndex: number) => {
  try {
    const response = await axios.post('/api/v1/lawful/store', null, {
      params: {
        lawId,
        sectionIndex
      }
    })
    console.log('Section stored successfully:', response.data)
    // You can add additional logic here, such as showing a success message to the user
  } catch (error) {
    console.error('Error storing section:', error)
    // You can add error handling logic here, such as showing an error message to the user
  }
}

watch(selectedLawId, (newId) => {
  if (newId) {
    fetchSections(newId)
  } else {
    sections.value = []
  }
})
</script>

<template>
  <header>
    <v-select
      v-if="laws.length > 0"
      v-model="selectedLawId"
      :items="laws"
      item-title="Name"
      item-value="Id"
      label="Select a law"
    ></v-select>
    <p v-else>No laws available</p>
  </header>

  <main>
    {{ selectedLawId }}
    <div v-if="sections.length > 0">
      <h2>Sections:</h2>
        <v-card v-for="(section, index) in sections" :key="index" class="mt-2">
          <v-card-title>{{ section.Title }}</v-card-title>
          <v-card-actions>
            <v-btn @click="storeSection(Number(selectedLawId), section.Index)" color="primary">Store</v-btn>
          </v-card-actions>
        </v-card>
    </div>
    <p v-else-if="selectedLawId">Loading sections...</p>
    <p v-else>Select a law to view its sections</p>
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
