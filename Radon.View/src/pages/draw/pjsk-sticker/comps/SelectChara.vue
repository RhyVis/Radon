<script lang="tsx" setup>
import { RefreshIcon } from "tdesign-icons-vue-next";
import { assembleSrc, type CharacterDefinition } from "@/pages/draw/pjsk-sticker/scripts/define";
import { useToggle } from "@vueuse/core";
import { useI18n } from "vue-i18n";

const { charaList, resEndpoint } = defineProps<{
  charaList: CharacterDefinition[];
  resEndpoint: string;
}>();
const emit = defineEmits(["select"]);

const { t } = useI18n();
const [visible, setVisible] = useToggle(false);
const renderIcon = () => <RefreshIcon />;

const handleDrawer = () => setVisible();
const handleSelect = (index: number) => {
  emit("select", index);
  handleDrawer();
};
</script>

<template>
  <t-button @click="handleDrawer">{{ t("button") }}</t-button>
  <t-drawer v-model:visible="visible" :footer="false" :header="t('header')" size="360px">
    <t-row :gutter="[2, 0]">
      <t-col v-for="(chara, index) in charaList" :key="index" :span="4">
        <span class="r-chara-select-option" @click="handleSelect(index)">
          <t-image
            class="r-chara-select-inner-img"
            :lazy="true"
            :loading="renderIcon"
            :src="assembleSrc(chara.img, resEndpoint)"
            fit="contain"
            style="height: 115px; width: 115px"
          />
        </span>
      </t-col>
    </t-row>
  </t-drawer>
</template>

<style scoped>
.r-chara-select-option {
  text-align: center;

  .r-chara-select-inner-img {
    max-width: 124px;
    max-height: 124px;
  }
}
</style>

<i18n locale="en">
button: "Select Character"
header: "Pick one!"
</i18n>

<i18n locale="zh-CN">
button: "选择角色"
header: "选择！"
</i18n>
