<template>
    <v-card class="mt-2">
      <v-card-title>{{ section.Title }}</v-card-title>
      <v-card-text>{{ section.Description }}</v-card-text>
      <v-card-text>{{ section.UserDescription }}</v-card-text>
      <v-card-actions>
        <v-btn v-if="section.CanStore" @click="storeSection" color="primary">Store</v-btn>
      </v-card-actions>
    </v-card>
  </template>
  
  <script setup lang="ts">
  import { defineProps } from 'vue';
  
  const props = defineProps({
    section: {
      type: Object,
      required: true
    },
    storeSection: {
      type: Function,
      required: true
    }
  });
  </script>
  
  <style scoped>
  /* Add any specific styles for SectionCard here */
  </style>