<script lang="ts" setup>
import { useI18n } from "vue-i18n";

type SimpleSelOpt = {
  label?: string;
  value?: string;
};

const { t } = useI18n();
const model = defineModel<string>({ required: true });
const emit = defineEmits(["update"]);
const {
  options,
  placeholder,
  loading = false,
  autoWidth = false,
} = defineProps<{
  options: SimpleSelOpt[];
  placeholder?: string;
  loading?: boolean;
  autoWidth?: boolean;
}>();
</script>

<template>
  <t-select
    v-model="model"
    :auto-width="autoWidth"
    :loading="loading"
    :loading-text="t('loading')"
    :placeholder="placeholder ?? t('placeholder')"
    @change="emit('update', model)"
  >
    <t-option
      v-for="(option, index) in options"
      :key="index"
      :label="option.label ?? option.value"
      :value="option.value"
    />
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
