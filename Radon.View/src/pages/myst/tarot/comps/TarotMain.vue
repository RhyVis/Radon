<script lang="tsx" setup>
import { intToRoman } from "@/pages/math/roman/scripts/romanNum";
import { type CardDisplay } from "@/pages/myst/tarot/scripts/define";
import { MoonIcon, SunnyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { card, index } = defineProps<{
  card: CardDisplay;
  index: number;
}>();
const { name, loc, img, rev, desc } = card.data;

const revClass = computed(() => ({ "r-tarot-img-rev": rev }));
const revText = computed(() => (rev ? "(逆位)" : "(正位)"));
const revDesc = computed(() => (rev ? desc.reverse : desc.upright));
const indexNum = computed(() => intToRoman(index + 1));

const handleImage = () => {
  // eslint-disable-next-line vue/no-mutating-props
  card.showImg = !card.showImg;
};
const handleDesc = () => {
  // eslint-disable-next-line vue/no-mutating-props
  card.showDesc = !card.showDesc;
};
const handleImageErr = (name: string) => {
  MessagePlugin.error(t("loadError", name));
};
const handleHash = () => {
  location.hash = `tarot-desc-${index}`;
};
</script>

<template>
  <t-card class="r-tarot-main-card-override mb-2" :header-bordered="true" :id="`tarot-main-${index}`">
    <template #title>
      <div class="text-primary r-no-select" @click="handleImage">
        {{ loc }}
      </div>
    </template>
    <template #actions>
      <div @click="handleHash">
        <i class="small text-black-50 r-no-select">{{ indexNum }} - {{ revText }} - </i>
        <MoonIcon v-if="rev" />
        <SunnyIcon v-else />
      </div>
    </template>
    <div v-show="card.showImg">
      <div class="r-tarot-main-div-full" @click="handleDesc">
        <div v-show="!card.showDesc">
          <t-image
            class="r-tarot-img-fit"
            :alt="name"
            :class="revClass"
            :lazy="true"
            :src="img"
            style="width: fit-content; max-width: 100%; height: 360px"
            fit="scale-down"
            loading="💫"
            @error="handleImageErr(name)"
          />
        </div>
        <div v-show="card.showDesc" style="text-align: left; align-items: center">
          <t-title :content="revDesc" level="h6" />
          <t-text :content="desc.desc.join('')" />
        </div>
      </div>
    </div>
  </t-card>
</template>

<style scoped lang="less">
.r-no-select {
  user-select: none;
}

.r-tarot-img-rev {
  transform: rotate(180deg);
}

.r-tarot-img-fit {
  margin: auto;
}

.r-tarot-main-div-full {
  flex: 1;
  height: 360px;
  text-align: center;
}

.r-tarot-main-card-override :deep(.t-card__body) {
  height: 400px;
}
</style>

<i18n locale="en">
loadError: "Load image failed: {0}"
</i18n>

<i18n locale="zh-CN">
loadError: "加载图片失败: {0}"
</i18n>
