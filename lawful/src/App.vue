<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'

interface Law {
  Id: string | number;
  Name: string;
}

interface Section {
  Title: string;
}

const laws = ref<Law[]>([])
const selectedLawId = ref<string | number | null>(null)
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
    <select v-if="laws.length > 0" v-model="selectedLawId">
      <option value="">Select a law</option>
      <option v-for="law in laws" :key="law.Id" :value="law.Id">{{ law.Name }}</option>
    </select>
    <p v-else>No laws available</p>
  </header>

  <main>
    {{ selectedLawId }}
    <div v-if="sections.length > 0">
      <h2>Sections:</h2>
      <ul>
        <li v-for="(section, index) in sections" :key="index">
          {{ section.Title }}
        </li>
      </ul>
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
