<script setup lang="ts">
type SimpleSelOpt = {
  label?: string;
  value?: string;
};

const model = defineModel<string>({ required: true });
const emit = defineEmits(["update"]);
defineProps<{
  options: SimpleSelOpt[];
  placeholder?: string;
  loading?: boolean;
  autoWidth?: boolean;
}>();

const { t } = useI18n();
</script>

<template>
  <t-select
    v-model="model"
    :placeholder="placeholder ?? t('placeholder')"
    :auto-width="autoWidth"
    :loading="loading ?? false"
    :loading-text="t('loading')"
    @change="emit('update', model)"
  >
    <t-option v-for="option in options" :key="option.value" :value="option.value" :label="option.label" />
  </t-select>
</template>

<i18n lang="yaml" locale="en">
placeholder: Please select
loading: Loading
</i18n>

<i18n lang="yaml" locale="zh-CN">
placeholder: 请选择
loading: 加载中
</i18n>
