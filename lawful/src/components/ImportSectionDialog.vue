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
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" @click="confirmImport">Confirm</v-btn>
        <v-btn @click="closeDialog">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, defineProps } from 'vue';

const props = defineProps<{
  isVisible: boolean;
  lawId: number;
  sectionId: number;
}>();

const emit = defineEmits(['close', 'import']);

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

