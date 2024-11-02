<script setup lang="ts">
type SimpSelOpt = {
  label?: string;
  value?: string;
};

const selected = defineModel("selected", { required: true });

const emit = defineEmits(["update"]);

defineProps<{
  options: SimpSelOpt[];
  placeholder?: string;
  loading?: boolean;
  autoWidth?: boolean;
}>();
</script>

<template>
  <t-select
    v-model="selected"
    :placeholder="placeholder"
    :auto-width="autoWidth"
    :loading="loading ?? false"
    loading-text="加载中"
    @change="emit('update', selected)"
  >
    <t-option v-for="option in options" :key="option.value" :value="option.value" :label="option.label" />
  </t-select>
</template>
