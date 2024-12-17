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
            <v-list-item v-for="reference in modifiedReferences" :key="reference.Name">
              <v-list-item-content>
                <v-list-item-title>{{ reference.Name }} ({{ reference.Type }})</v-list-item-title>
                <v-autocomplete v-model="reference.MappedName" :items="reference.PossibleValues" label="Possible Values" />
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
const modifiedReferences = ref<ReferenceDto[]>([]); // Local state for modified references

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

watch(references, (newReferences) => {
    modifiedReferences.value = newReferences.map(reference => ({
        ...reference,
        possibleValues: [...reference.PossibleValues] // Initialize with possible values
    }));
});

const closeDialog = () => {
  emit('close');
};

const confirmImport = () => {
  const referencesToSend = modifiedReferences.value.map(({ PossibleValues, ...rest }) => rest); // Omit possibleValues
  emit('import', { lawId: props.lawId, sectionId: props.sectionId, references: referencesToSend }); // Pass modified references array without possibleValues
  closeDialog();
};
</script>

<style scoped>
/* Add any styles for the dialog here */
</style>
