<!-- lawful/src/components/ImportSectionDialog.vue -->
<template>
  <v-dialog v-model="isVisible" max-width="600px">
    <v-card>
      <v-card-title>
        <span class="headline">Import Section</span>
      </v-card-title>
      <v-card-text>
        <p>Are you sure you want to import this section?</p>
        <p>Law ID: {{ lawId }}</p>
        <p>Section ID: {{ sectionId }}</p>
        <v-list>
          <v-list-item-group>
            <v-list-item v-for="reference in references" :key="reference.Name">
              <v-list-item-content>
                <v-list-item-title>{{ reference.Name }} ({{ reference.Type }})</v-list-item-title>
                <v-autocomplete :items="reference.possibleValues" label="Possible Values" />
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" @click="confirmImport">Confirm</v-btn>
        <v-btn @click="closeDialog">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, defineProps, defineEmits, onMounted, watch } from 'vue';
import { fetchReferences } from '@/services/lawful';
import type { ReferenceDto } from '@/services/dto';

const props = defineProps<{
  isVisible: boolean;
  lawId: number;
  sectionId: number;
}>();

const emit = defineEmits(['close', 'import']);

const references = ref<ReferenceDto[]>([]); // Use the ReferenceDto type

const fetchReferenceData = async () => {
    try {
        references.value = await fetchReferences(props.sectionId); // Fetch references using sectionId
    } catch (error) {
        console.error('Error fetching references:', error);
    }
};

onMounted(() => {
    fetchReferenceData(); // Fetch references when the component is mounted
});

// New watch effect to fetch data when sectionId changes
watch(() => props.sectionId, (newSectionId) => {
    fetchReferenceData(); // Fetch references when sectionId changes
});

const closeDialog = () => {
  emit('close');
};

const confirmImport = () => {
  emit('import', { lawId: props.lawId, sectionId: props.sectionId });
  closeDialog();
};
</script>

<style scoped>
/* Add any styles for the dialog here */
</style>
