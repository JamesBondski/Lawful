<script setup lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'

import { ref, onMounted } from 'vue'
import axios from 'axios'

const laws = ref([])

onMounted(async () => {
  try {
    const response = await axios.get('/api/v1/lawful/laws')
    laws.value = response.data
  } catch (error) {
    console.error('Error fetching laws:', error)
  }
})

</script>

<template>
  <header>
    <select v-if="laws.length > 0">
      <option v-for="law in laws" :key="law" :value="law">{{ law }}</option>
    </select>
    <p v-else>No laws available</p>
  </header>

  <main>
    <TheWelcome />
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
